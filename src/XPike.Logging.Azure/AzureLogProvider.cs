using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.Collections.Generic;
using System.Threading.Tasks;
using XPike.Configuration;
using XPike.Settings;

namespace XPike.Logging.Azure
{
    public class AzureLogProvider
        : IAzureLogProvider
    {
        private readonly AzureLogSettings _settings;
        private readonly TelemetryClient _client;

        public AzureLogProvider(IConfigurationService configService, ISettings<AzureLogSettings> settings)
        {
            _settings = settings.Value;
            _client = new TelemetryClient(new TelemetryConfiguration(_settings.InstrumentationKey));
        }

        private void PopulateTelemetry(IDictionary<string, string> metadata, LogEvent logEvent)
        {
            foreach(var item in logEvent.Metadata)
                metadata[item.Key] = item.Value;

            metadata[nameof(logEvent.Category)] = logEvent.Category;
            metadata[nameof(logEvent.Location)] = logEvent.Location;
        }

        private SeverityLevel GetSeverity(LogEvent logEvent)
        {
            switch (logEvent?.LogLevel ?? LogLevel.Error)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return SeverityLevel.Verbose;
                case LogLevel.Log:
                case LogLevel.Info:
                    return SeverityLevel.Information;
                case LogLevel.Warning:
                    return SeverityLevel.Warning;
                case LogLevel.Error:
                default:
                    return SeverityLevel.Error;
            }
        }
        public Task<bool> WriteAsync(LogEvent logEvent) =>
            Task.Run(() => Write(logEvent));

        public bool Write(LogEvent logEvent)
        {
            if (logEvent.Exception == null)
            {
                var telemetry = new TraceTelemetry(logEvent.Message, GetSeverity(logEvent))
                {
                    Timestamp = logEvent.Timestamp
                };

                PopulateTelemetry(telemetry.Properties, logEvent);

                _client.TrackTrace(telemetry);
                return true;
            }
            else
            {
                var telemetry = new ExceptionTelemetry(logEvent.Exception)
                {
                    Message = logEvent.Message,
                    SeverityLevel = GetSeverity(logEvent),
                    Timestamp = logEvent.Timestamp
                };

                PopulateTelemetry(telemetry.Properties, logEvent);

                _client.TrackException(telemetry);
                return true;
            }
        }
    }
}
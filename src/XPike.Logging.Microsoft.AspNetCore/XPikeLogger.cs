using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public class XPikeLogger
        : ILogger
    {
        private readonly XPikeLoggerProvider _provider;
        private readonly string _categoryName;

        public XPikeLogger(XPikeLoggerProvider provider, string categoryName)
        {
            _provider = provider;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) =>
            new LoggerExternalScopeProvider().Push(state);

        public bool IsEnabled(global::Microsoft.Extensions.Logging.LogLevel logLevel) =>
            true;

        public LogLevel GetLogLevel(global::Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            switch(logLevel)
            {
                case global::Microsoft.Extensions.Logging.LogLevel.Trace:
                    return LogLevel.Trace;
                case global::Microsoft.Extensions.Logging.LogLevel.Debug:
                    return LogLevel.Debug;
                case global::Microsoft.Extensions.Logging.LogLevel.Information:
                    return LogLevel.Info;
                case global::Microsoft.Extensions.Logging.LogLevel.Warning:
                    return LogLevel.Warning;
                case global::Microsoft.Extensions.Logging.LogLevel.Error:
                case global::Microsoft.Extensions.Logging.LogLevel.Critical:
                case global::Microsoft.Extensions.Logging.LogLevel.None:
                default:
                    return LogLevel.Error;
            }
        }

        public void Log<TState>(global::Microsoft.Extensions.Logging.LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (formatter == null)
                return;

            var metadata = new Dictionary<string, string>();
            metadata[nameof(eventId)] = eventId.ToString();

            if (state is IReadOnlyList<KeyValuePair<string, object>> values)
            {
                foreach (var entry in values)
                {
                    try
                    {
                        if (entry.Value is Dictionary<string, string> dictionary)
                        {
                            foreach (var kvp in dictionary)
                                metadata.Add(kvp.Key, kvp.Value?.ToString());
                        }
                        else if (entry.Value is KeyValuePair<string, string> keyValuePair)
                        {
                            metadata.Add(keyValuePair.Key, keyValuePair.Value);
                        }
                        else if (entry.Value is KeyValuePair<string, string>[] kvps)
                        {
                            foreach (var kvp in kvps)
                                metadata.Add(kvp.Key, kvp.Value?.ToString());
                        }
                        else if (entry.Value is KeyValuePair<string, object> kvp)
                        {
                            metadata.Add(keyValuePair.Key, JsonConvert.SerializeObject(keyValuePair.Value));
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                    }
                }
            }

            _provider.Write(new LogEvent
            {
                Message = formatter(state, exception),
                Metadata = metadata,
                Category = _categoryName,
                Exception = exception,
                LogLevel = GetLogLevel(logLevel),
                Location = nameof(XPikeLogger),
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
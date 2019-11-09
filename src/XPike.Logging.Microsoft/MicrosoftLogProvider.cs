using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILoggerFactory = global::Microsoft.Extensions.Logging.ILoggerFactory;

namespace XPike.Logging.Microsoft
{
    public class MicrosoftLogProvider
        : IMicrosoftLogProvider
    {
        private readonly ConcurrentDictionary<string, ILogger> _loggers;
        private readonly ILoggerFactory _factory;

        public MicrosoftLogProvider(ILoggerFactory factory)
        {
            _factory = factory;
            _loggers = new ConcurrentDictionary<string, ILogger>();
        }

        private ILogger GetLogger(LogEvent logEvent) =>
            _loggers.GetOrAdd(logEvent.Category, _factory.CreateLogger);
            
        public Task<bool> WriteAsync(LogEvent logEvent)
        {
            var logger = GetLogger(logEvent);
            var metadata = new Dictionary<string, string>();

            foreach (var item in logEvent.Metadata)
                metadata[item.Key] = item.Value;

            metadata[nameof(logEvent.Timestamp)] = logEvent.Timestamp.ToString();
            metadata[nameof(logEvent.Location)] = logEvent.Location;

            var sb = new StringBuilder();
            sb.Append(logEvent.Message);

            var first = false;
            foreach (var item in metadata)
            {
                if (!first)
                    sb.Append(", ");

                sb.Append($"{item.Key}={{{item.Key}}}");
                
                first = false;
            }

            var message = sb.ToString();
            var values = metadata.Values.ToArray();

            switch(logEvent.LogLevel)
            {
                case LogLevel.Trace:
                    if (logEvent.Exception == null)
                    {
                        logger.LogTrace(message, values);
                    }
                    else
                    {
                        logger.LogTrace(logEvent.Exception, message, values);
                    }

                    break;
                case LogLevel.Debug:
                    if (logEvent.Exception == null)
                    {
                        logger.LogDebug(message, values);
                    }
                    else
                    {
                        logger.LogDebug(logEvent.Exception, message, values);
                    }

                    break;
                case LogLevel.Log:
                case LogLevel.Info:
                    if (logEvent.Exception == null)
                    {
                        logger.LogInformation(message, values);
                    }
                    else
                    {
                        logger.LogInformation(logEvent.Exception, message, values);
                    }

                    break;
                case LogLevel.Warning:
                    if (logEvent.Exception == null)
                    {
                        logger.LogWarning(message, values);
                    }
                    else
                    {
                        logger.LogWarning(logEvent.Exception, message, values);
                    }

                    break;
                case LogLevel.Error:
                default:
                    if (logEvent.Exception == null)
                    {
                        logger.LogError(message, values);
                    }
                    else
                    {
                        logger.LogError(logEvent.Exception, message, values);
                    }

                    break;
            }

            return Task.FromResult(true);
        }
    }
}
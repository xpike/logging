using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace XPike.Logging.Microsoft.AspNetCore
{
    /// <summary>
    /// An implementation of Microsoft.Extensions.Logging ILogger that writes to the xPike Logging system.
    /// Implements the <see cref="Microsoft.Extensions.Logging.ILogger" />
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Logging.ILogger" />
    public class XPikeLogger
        : ILogger
    {
        private readonly XPikeLoggerProvider _provider;
        private readonly string _categoryName;

        /// <summary>
        /// Initializes a new instance of the <see cref="XPikeLogger"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="categoryName">Name of the category.</param>
        public XPikeLogger(XPikeLoggerProvider provider, string categoryName)
        {
            _provider = provider;
            _categoryName = categoryName;
        }

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">The type of the t state.</typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An IDisposable that ends the logical operation scope on dispose.</returns>
        public IDisposable BeginScope<TState>(TState state) =>
            new LoggerExternalScopeProvider().Push(state);

        /// <summary>
        /// Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns><c>true</c> if enabled.</returns>
        public bool IsEnabled(global::Microsoft.Extensions.Logging.LogLevel logLevel) =>
            true;

        /// <summary>
        /// Gets the log level.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <returns>LogLevel.</returns>
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

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TState">The type of the t state.</typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
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
                                metadata.Add(kvp.Key, kvp.Value);
                        }
                        else if (entry.Value is KeyValuePair<string, string> keyValuePair)
                        {
                            metadata.Add(keyValuePair.Key, keyValuePair.Value);
                        }
                        else if (entry.Value is KeyValuePair<string, string>[] kvps)
                        {
                            foreach (var kvp in kvps)
                                metadata.Add(kvp.Key, kvp.Value);
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
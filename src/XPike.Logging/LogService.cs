using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using XPike.Configuration;

namespace XPike.Logging
{
    /// <summary>
    /// Dedfault LogService implementation.
    /// Implements the <see cref="XPike.Logging.ILogService" />
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="XPike.Logging.ILogService" />
    /// <seealso cref="System.IDisposable" />
    public class LogService
        : ILogService,
          IDisposable
    {
        private readonly IConfig<LogServiceConfig> _config;
        private readonly IList<ILogProvider> _providers;
        private readonly ITraceContextAccessor _contextAccessor;

        private BlockingCollection<LogEvent> _eventQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogService"/> class.
        /// </summary>
        /// <param name="providers">The providers.</param>
        /// <param name="configManager">The settings manager.</param>
        /// <param name="contextAccessor">The trace context accessor.</param>
        public LogService(IEnumerable<ILogProvider> providers,
            IConfigManager<LogServiceConfig> configManager,
            ITraceContextAccessor contextAccessor)
        {
            _config = configManager.GetConfigOrDefault(new LogServiceConfig
            {
                MaxQueueLength = 5000,
                EnqueueTimeoutMs = 10,
                LogLevel = LogLevel.Log
            });

            _providers = providers.ToList();
            _eventQueue = new BlockingCollection<LogEvent>();
            _contextAccessor = contextAccessor;

            // NOTE: Intentional fire-and-forget.
            _ = Task.Run(async () => await QueueProcessorAsync().ConfigureAwait(false));
        }

        /// <inheritdoc />
        public virtual bool Debug(string message,
                                  Dictionary<string, string> metadata = null,
                                  string category = LogServiceDefaults.DEFAULT_CATEGORY,
                                  [CallerMemberName] string location = null) =>
            Write(LogLevel.Debug,
                message,
                metadata,
                null,
                category,
                location);

        /// <inheritdoc />
        public virtual bool Error(string message,
                                  Exception exception = null,
                                  Dictionary<string, string> metadata = null,
                                  string category = LogServiceDefaults.DEFAULT_CATEGORY,
                                  [CallerMemberName] string location = null) =>
            Write(LogLevel.Error,
                message,
                metadata,
                exception,
                category,
                location);

        /// <inheritdoc />
        public virtual bool Info(string message,
                                 Dictionary<string, string> metadata = null,
                                 string category = LogServiceDefaults.DEFAULT_CATEGORY,
                                 [CallerMemberName] string location = null) =>
            Write(LogLevel.Info,
                message,
                metadata,
                null,
                category,
                location);

        /// <inheritdoc />
        public virtual bool Log(string message,
                                Dictionary<string, string> metadata = null,
                                string category = LogServiceDefaults.DEFAULT_CATEGORY,
                                [CallerMemberName] string location = null) =>
            Write(LogLevel.Log,
                  message,
                  metadata,
                  null,
                  category,
                  location);

        /// <inheritdoc />
        public virtual bool Trace(string message,
                                  Dictionary<string, string> metadata = null,
                                  string category = LogServiceDefaults.DEFAULT_CATEGORY,
                                  [CallerMemberName] string location = null) =>
            Write(LogLevel.Trace,
                  message,
                  metadata,
                  null,
                  category,
                  location);

        /// <inheritdoc />
        public virtual bool Warn(string message,
                                 Exception exception = null,
                                 Dictionary<string, string> metadata = null,
                                 string category = LogServiceDefaults.DEFAULT_CATEGORY,
                                 [CallerMemberName] string location = null) =>
            Write(LogLevel.Warning,
                  message,
                  metadata,
                  exception,
                  category,
                  location);

        /// <inheritdoc />
        public virtual bool Write(LogEvent logEvent)
        {
            if (logEvent == null)
                return false;

            if (logEvent.LogLevel <= _config.CurrentValue.LogLevel)
                return _eventQueue.TryAdd(logEvent, _config.CurrentValue.EnqueueTimeoutMs);

            return true;
        }

        /// <inheritdoc />
        public virtual async Task QueueProcessorAsync()
        {
            foreach (var ev in _eventQueue.GetConsumingEnumerable())
                await WriteAsync(ev).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public bool Write(LogLevel logLevel,
                          string message,
                          Dictionary<string, string> metadata = null,
                          Exception exception = null,
                          string category = LogServiceDefaults.DEFAULT_CATEGORY,
                          [CallerMemberName] string location = null) =>
            Write(PopulateLogEvent(logLevel, message, metadata, exception, category, location));

        /// <inheritdoc />
        public virtual async Task<bool> WriteAsync(LogEvent logEvent) =>
            (await Task.WhenAll(_providers.Select(x => x.WriteAsync(logEvent))).ConfigureAwait(false)).All(x => x);

        /// <inheritdoc />
        public virtual Task<bool> WriteAsync(LogLevel logLevel,
                                             string message,
                                             Dictionary<string, string> metadata = null,
                                             Exception exception = null,
                                             string category = LogServiceDefaults.DEFAULT_CATEGORY,
                                             [CallerMemberName] string location = null) =>
            WriteAsync(PopulateLogEvent(logLevel, message, metadata, exception, category, location));

        /// <inheritdoc />
        public virtual LogEvent PopulateLogEvent(LogLevel logLevel,
            string message,
            Dictionary<string, string> metadata = null,
            Exception exception = null,
            string category = LogServiceDefaults.DEFAULT_CATEGORY,
            [CallerMemberName] string location = null)
        {
            var actualMetadata = new Dictionary<string, string>();

            try
            {
                var context = _contextAccessor.TraceContext;
                if (context?.Items != null)
                    foreach (var item in context.Items)
                        actualMetadata[item.Key] = item.Value;
            }
            catch
            {
            }

            if (metadata != null)
                foreach (var item in metadata)
                    actualMetadata[item.Key] = item.Value;

            return new LogEvent
            {
                Exception = exception,
                Location = location,
                LogLevel = logLevel,
                Message = message,
                Metadata = actualMetadata,
                Category = category,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LogService"/> class.
        /// </summary>
        ~LogService() =>
            Dispose(false);

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_eventQueue != null)
            {
                if (!_eventQueue.IsAddingCompleted)
                    _eventQueue.CompleteAdding();

                if (disposing)
                {
                    while (true)
                        if (_eventQueue.IsCompleted)
                            break;

                    _eventQueue.Dispose();
                    _eventQueue = null;
                }
            }
        }
    }
}
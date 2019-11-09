using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace XPike.Logging
{
    public class LogWriter<TSource>
        : ILog<TSource>
        where TSource : class
    {
        private readonly ILogService _logService;
        private readonly string _source;

        public LogWriter(ILogService logService)
        {
            _logService = logService;
            _source = typeof(TSource).FullName;
        }

        public virtual bool Debug(string message,
                                  Dictionary<string, string> metadata = null,
                                  [CallerMemberName] string location = null) =>
            _logService.Debug(message,
                              metadata,
                              _source,
                              location);

        public virtual bool Error(string message,
                                  Exception exception = null,
                                  Dictionary<string, string> metadata = null,
                                  [CallerMemberName] string location = null) =>
            _logService.Error(message,
                              exception,
                              metadata,
                              _source,
                              location);

        public virtual bool Info(string message,
                                 Dictionary<string, string> metadata = null,
                                 [CallerMemberName] string location = null) =>
            _logService.Info(message,
                             metadata,
                             _source,
                             location);

        public virtual bool Log(string message,
                                Dictionary<string, string> metadata = null,
                                [CallerMemberName] string location = null) =>
            _logService.Log(message,
                            metadata,
                            _source,
                            location);

        public virtual bool Trace(string message,
                                  Dictionary<string, string> metadata = null,
                                  [CallerMemberName] string location = null) =>
            _logService.Trace(message,
                              metadata,
                              _source,
                              location);

        public virtual bool Warn(string message,
                                 Exception exception = null,
                                 Dictionary<string, string> metadata = null,
                                 [CallerMemberName] string location = null) =>
            _logService.Warn(message,
                             exception,
                             metadata,
                             _source,
                             location);

        public virtual bool Write(LogEvent logEvent)
        {
            if (logEvent == null)
                return false;

            logEvent.Category = _source;

            return _logService.Write(logEvent);
        }

        public bool Write(LogLevel logLevel,
                          string message,
                          Dictionary<string, string> metadata = null,
                          Exception exception = null,
                          [CallerMemberName] string location = null) =>
            _logService.Write(logLevel, message, metadata, exception, _source, location);
    }
}
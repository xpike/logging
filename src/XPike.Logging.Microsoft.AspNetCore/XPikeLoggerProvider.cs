using System;
using Microsoft.Extensions.Logging;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public class XPikeLoggerProvider
        : ILoggerProvider
    {
        // NOTE: HACK
        public static ILogService LogService { get; set; }

        public XPikeLoggerProvider()
        {
        }

        public ILogger CreateLogger(string categoryName) =>
            new XPikeLogger(this, categoryName);

        public bool Write(LogEvent logEvent)
        {
            if (LogService == null)
                return false;

            return LogService.Write(logEvent);
        }

        ~XPikeLoggerProvider() =>
            Dispose(false);

        protected virtual void Dispose(bool disposing)
        {
            // NOTE: Intentional no-op.
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

using System;
using Microsoft.Extensions.Logging;

namespace XPike.Logging.Microsoft.AspNetCore
{
    /// <summary>
    /// A Microsoft.Exensions.Logging ILoggerProvider that writes logs to the xPike Logging system.
    /// Implements the <see cref="Microsoft.Extensions.Logging.ILoggerProvider" />
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Logging.ILoggerProvider" />
    public class XPikeLoggerProvider
        : ILoggerProvider
    {
        // NOTE: HACK        
        /// <summary>
        /// Gets or sets the log service.
        /// </summary>
        /// <value>The log service.</value>
        public static ILogService LogService { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XPikeLoggerProvider"/> class.
        /// </summary>
        public XPikeLoggerProvider()
        {
        }

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>ILogger.</returns>
        public ILogger CreateLogger(string categoryName) =>
            new XPikeLogger(this, categoryName);

        /// <summary>
        /// Writes the specified log event.
        /// </summary>
        /// <param name="logEvent">The log event.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Write(LogEvent logEvent)
        {
            if (LogService == null)
                return false;

            return LogService.Write(logEvent);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="XPikeLoggerProvider"/> class.
        /// </summary>
        ~XPikeLoggerProvider() =>
            Dispose(false);

        protected virtual void Dispose(bool disposing)
        {
            // NOTE: Intentional no-op.
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

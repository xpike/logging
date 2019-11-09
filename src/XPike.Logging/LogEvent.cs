using System;
using System.Collections.Generic;

namespace XPike.Logging
{
    /// <summary>
    /// Encapsulates all of the details necessary to record a logged event.
    /// </summary>
    public class LogEvent
    {
        /// <summary>
        /// The logged message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The location within the code from which the log entry was initiated.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Additional data relative to this logged event.
        /// </summary>
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// The timestamp at which the log entry was generated.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The severity of this log entry.
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// The exception, if any, that resulted in this log entry.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// An optional category for the event, such as typeof(TSource).FullName.
        /// </summary>
        public string Category { get; set; }
    }
}
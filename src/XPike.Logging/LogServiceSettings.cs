namespace XPike.Logging
{
    /// <summary>
    /// Settings for the LogService
    /// </summary>
    public class LogServiceSettings
    {
        /// <summary>
        /// Gets or sets the maximum length of the in-memory queue for asynchronous logging.
        /// </summary>
        /// <value>The maximum length of the queue.</value>
        public int MaxQueueLength { get; set; }

        /// <summary>
        /// Gets or sets the enqueue timeout ms.
        /// </summary>
        /// <value>The enqueue timeout ms.</value>
        public int EnqueueTimeoutMs { get; set; }

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        /// <value>The log level.</value>
        public LogLevel LogLevel { get; set; }
    }
}
using System.Collections.Generic;

namespace XPike.Logging
{
    /// <summary>
    /// Represents tracing metadata to be added to each log entry. Metadata added here is not distributed.
    /// </summary>
    public interface ITraceContext
    {
        /// <summary>
        /// A readonly snapshot of the metadata items in the context.
        /// </summary>
        IReadOnlyDictionary<string, string> Items { get; }

        /// <summary>
        /// Sets the value for the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(string key, string value);

        /// <summary>
        /// Gets the value of the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);
    }
}
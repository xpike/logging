using System;
using System.Collections.Generic;

namespace XPike.Logging
{
    /// <summary>
    /// NOTE: This implementation is not currently thread-safe when modifying the Items collection.
    /// </summary>
    public class TraceContext
        : ITraceContext
    {
        public const string TRACE_ID_KEY = "traceId";
        
        public IDictionary<string, string> Items { get; }

        public TraceContext()
            : this(new Dictionary<string, string>())
        {
        }

        protected internal TraceContext(IDictionary<string, string> items)
        {
            Items = items;

            items[TRACE_ID_KEY] = Guid.NewGuid().ToString();
        }

        public void Set(string key, string value) =>
            Items[key] = value;
    }
}
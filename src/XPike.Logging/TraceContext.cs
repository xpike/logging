using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace XPike.Logging
{
    /// <summary>
    /// Default implementation of ITraceContext
    /// </summary>
    public class TraceContext
        : ITraceContext
    {
        /// <summary>
        /// Key used to track the traceId.
        /// </summary>
        public const string TRACE_ID_KEY = "traceId";

        private readonly ConcurrentDictionary<string, string> _items;

        /// <summary>
        /// Initialized a new empty TraceContext.
        /// </summary>
        public TraceContext()
            : this(null)
        {
        }

        /// <summary>
        /// Initialized a new TraceContext using data from the supplied IDictionary.
        /// Items are copied into the context.
        /// </summary>
        /// <param name="items"></param>
        protected internal TraceContext(IDictionary<string, string> items)
        {
            if (items == null)
                _items = new ConcurrentDictionary<string, string>();
            else
                _items =new ConcurrentDictionary<string, string>(items);

            _items.TryAdd(TRACE_ID_KEY, Guid.NewGuid().ToString());
        }

        ///<inheritdoc />
        public IReadOnlyDictionary<string, string> Items 
        {
#if NETSTD || NET46 //as of net46, ConcurrentDictionary<,> implements IReadOnlyDictionary<,>
            get => _items;
#else
            get => new Dictionary<string, string>(_items);
#endif
        }

        /// <summary>
        /// Sets the value of the specified key in a thread-safe manner.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Set(string key, string value)
        {
            _items.AddOrUpdate(key, value, (k, oldValue) => value);
        }

        /// <summary>
        /// Gets the value of the specified key in a thread-safe manner.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            _items.TryGetValue(key, out string value);
            return value;
        }
        
    }
}
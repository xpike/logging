#if NETSTD
using System.Threading;
#elif NETFX
using System.Runtime.Remoting.Messaging;
#endif

namespace XPike.Logging
{
    public class TraceContextAccessor
        : ITraceContextAccessor
    {
#if NETSTD
        private static readonly AsyncLocal<ITraceContext> _localizer = new AsyncLocal<ITraceContext>();
#endif

        private readonly ITraceContextProvider _provider;

#if NETSTD
        public ITraceContext TraceContext =>
            _localizer.Value ?? (_localizer.Value = _provider.CreateContext());
#elif NETFX
        public ITraceContext TraceContext
        {
            get
            {
                var context = (ITraceContext) CallContext.LogicalGetData(GetType().ToString());

                if (context == null)
                {
                    context = _provider.CreateContext();
                    CallContext.LogicalSetData(GetType().ToString(), context);
                }

                return context;
            }
        }
#endif

        public TraceContextAccessor(ITraceContextProvider provider)
        {
            _provider = provider;
        }
    }
}
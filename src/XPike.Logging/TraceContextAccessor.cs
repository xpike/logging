using System.Threading;

namespace XPike.Logging
{
    public class TraceContextAccessor
        : ITraceContextAccessor
    {
        private static readonly AsyncLocal<ITraceContext> _localizer = new AsyncLocal<ITraceContext>();

        private readonly ITraceContextProvider _provider;

        public ITraceContext TraceContext =>
            _localizer.Value ?? (_localizer.Value = _provider.CreateContext());

        public TraceContextAccessor(ITraceContextProvider provider)
        {
            _provider = provider;
        }
    }
}
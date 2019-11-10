namespace XPike.Logging
{
    public interface ITraceContextAccessor
    {
        ITraceContext TraceContext { get; }
    }
}
using System.Collections.Generic;

namespace XPike.Logging
{
    public interface ITraceContextProvider
    {
        ITraceContext CreateContext();

        IDictionary<string, string> Globals { get; }

        void SetGlobal(string key, string value);
    }
}
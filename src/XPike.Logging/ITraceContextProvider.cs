using System.Collections.Generic;

namespace XPike.Logging
{
    public interface ITraceContextProvider
    {
        ITraceContext CreateContext();

        IReadOnlyDictionary<string, string> Globals { get; }

        void SetGlobal(string key, string value);

        string GetGlobal(string key);
    }
}
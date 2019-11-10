using System.Collections.Generic;

namespace XPike.Logging
{
    public interface ITraceContext
    {
        IDictionary<string, string> Items { get; }
        
        //IDictionary<string,string> Globals { get; }

        void Set(string key, string value);

        //void SetGlobal(string key, string value);
    }
}
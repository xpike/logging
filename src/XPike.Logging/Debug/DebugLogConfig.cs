using System;
using System.Runtime.Serialization;
using XPike.Logging.Console;

namespace XPike.Logging.Debug
{
    [Serializable]
    [DataContract]
    public class DebugLogConfig
        : ConsoleLogConfig
    {
    }
}
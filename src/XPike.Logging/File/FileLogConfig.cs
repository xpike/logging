using System;
using System.Runtime.Serialization;
using XPike.Logging.Console;

namespace XPike.Logging.File
{
    [Serializable]
    [DataContract]
    public class FileLogConfig
        : ConsoleLogConfig
    {
        [DataMember]
        public string Path { get; set; }
    }
}
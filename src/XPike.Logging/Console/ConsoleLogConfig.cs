using System;
using System.Runtime.Serialization;

namespace XPike.Logging.Console
{
    [Serializable]
    [DataContract]
    public class ConsoleLogConfig
    {
        [DataMember]
        public bool ShowStackTraces { get; set; }

        [DataMember]
        public bool ShowMetadata { get; set; }

        [DataMember]
        public bool Enabled { get; set; }
    }
}
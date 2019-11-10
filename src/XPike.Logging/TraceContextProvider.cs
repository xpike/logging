using System;
using System.Collections.Generic;
using System.Linq;

namespace XPike.Logging
{
    public class TraceContextProvider
        : ITraceContextProvider
    {
        public IDictionary<string, string> Globals { get; protected set; } = new Dictionary<string, string>();

        public TraceContextProvider()
            : this(new Dictionary<string, string>())
        {
        }

        protected TraceContextProvider(IDictionary<string, string> globals)
        {
            Globals = globals;

            SetGlobal("UserName", Environment.UserName);
            SetGlobal("UserDomainName", Environment.UserDomainName);
            SetGlobal("OperatingSystem", Environment.OSVersion.VersionString);
            SetGlobal("Is64BitOperatingSystem", Environment.Is64BitOperatingSystem.ToString());
            SetGlobal("ProcessorCount", Environment.ProcessorCount.ToString());
            SetGlobal("ClrVersion", Environment.Version.ToString());
            SetGlobal("Is64BitProcess", Environment.Is64BitProcess.ToString());
            SetGlobal("MachineName", Environment.MachineName);
            SetGlobal("UtcOffset", DateTimeOffset.Now.Offset.ToString());
            SetGlobal("HostName", NetUtil.GetHostname());
            SetGlobal("IpAddresses", $"Local={NetUtil.GetLocalIp()};Public={NetUtil.GetPublicIp()}");
        }

        public ITraceContext CreateContext() =>
            new TraceContext(Globals.ToDictionary(x => x.Key, x => x.Value));

        public void SetGlobal(string key, string value) =>
            Globals[key] = value;
    }
}
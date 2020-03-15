using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace XPike.Logging
{
    public class TraceContextProvider
        : ITraceContextProvider
    {
        private readonly ConcurrentDictionary<string, string> _globals;

        public TraceContextProvider()
            : this(new ConcurrentDictionary<string, string>())
        {
        }

        protected TraceContextProvider(IDictionary<string, string> globals)
        {
            if (globals == null)
                _globals = new ConcurrentDictionary<string, string>();
            else
                _globals =new ConcurrentDictionary<string, string>(globals);

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

        public IReadOnlyDictionary<string, string> Globals
        {
#if NETSTD || NET46 //as of net46, ConcurrentDictionary<,> implements IReadOnlyDictionary<,>
            get => _globals;
#else
            get => new Dictionary<string, string>(_globals);
#endif
        } 

        
        public ITraceContext CreateContext() =>
            new TraceContext(Globals.ToDictionary(x => x.Key, x => x.Value));

        public void SetGlobal(string key, string value)
        {
            _globals.AddOrUpdate(key, value, (k, oldValue) => value);
        }

        public string GetGlobal(string key)
        {
            _globals.TryGetValue(key, out string value);
            return value;
        }
    }
}
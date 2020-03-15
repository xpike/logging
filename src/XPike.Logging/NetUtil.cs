using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace XPike.Logging
{
    public static class NetUtil
    {
        private static volatile string _hostname = null;
        private static volatile string _localIp = null;
        private static volatile string _publicIp = null;

        private static readonly object _hostnameSync = new object();
        private static readonly object _localIpSync = new object();
        private static readonly object _publicIpSync = new object();

        public static string GetHostname()
        {
            if (_hostname == null)
            {
                lock (_hostnameSync)
                {
                    if (_hostname == null)
                        _hostname = Dns.GetHostName();
                }
            }

            return _hostname;
        }

        public static string GetLocalIp()
        {
            if (_localIp == null)
            {
                lock (_localIpSync)
                {
                    if (_localIp == null)
                    {
                        try
                        {
                            _localIp = Dns.GetHostEntry(Dns.GetHostName())
                                .AddressList
                                .FirstOrDefault(address => address.AddressFamily == AddressFamily.InterNetwork)
                                .ToString();
                        }
                        catch (Exception)
                        {
                            _localIp = "0.0.0.0";
                        }
                    }
                }
            }

            return _localIp;
        }

        public static string GetPublicIp()
        {
            if (_publicIp == null)
            {
                lock (_publicIpSync)
                {
                    if (_publicIp == null)
                    {
                        try
                        {
                            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                            {
                                socket.Connect("8.8.8.8", 65530);
                                var endPoint = socket.LocalEndPoint as IPEndPoint;
                                _publicIp = endPoint.Address.ToString();
                            }
                        }
                        catch (Exception)
                        {
                            _publicIp = "0.0.0.0";
                        }
                    }
                }
            }

            return _publicIp;
        }
    }
}
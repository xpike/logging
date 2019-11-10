using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace XPike.Logging
{
    public static class NetUtil
    {
        private static string _hostname = null;
        private static string _localIp = null;
        private static string _publicIp = null;

        public static string GetHostname() =>
            _hostname ?? (_hostname = Dns.GetHostName());

        public static string GetLocalIp()
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

            return _localIp;
        }

        public static string GetPublicIp()
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
                catch (Exception ex)
                {
                    _publicIp = "0.0.0.0";
                }
            }

            return _publicIp;
        }
    }
}
using Microsoft.Extensions.Logging;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public static class ILoggerFactoryExtensions
    {
        public static ILoggerFactory AddXPikeLogging(this ILoggerFactory factory)
        {
            factory.AddProvider(new XPikeLoggerProvider());
            return factory;
        }
    }
}
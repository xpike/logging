using XPike.IoC;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public static class IDependencyProviderExtensions
    {
        public static IDependencyProvider UseXPikeLogging(this IDependencyProvider services)
        {
            // NOTE: HACK
            XPikeLoggerProvider.LogService = services.ResolveDependency<ILogService>();
            return services;
        }
    }
}
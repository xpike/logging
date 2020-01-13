using XPike.IoC;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public static class IDependencyProviderExtensions
    {
        /// <summary>
        /// Enables XPike Logging.
        ///
        /// NOTE: You must also call either IHostBuilder.AddXPikeLogging() or
        /// IHostBuilder.UseXPikeLogging() in Program.cs.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IDependencyProvider UseXPikeLogging(this IDependencyProvider services)
        {
            // NOTE: HACK
            XPikeLoggerProvider.LogService = services.ResolveDependency<ILogService>();
            return services;
        }
    }
}
using XPike.IoC;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public static class IDependencyProviderExtensions
    {
        /// <summary>
        /// Enables XPike Logging.
        ///
        /// NOTE: You must also call ILoggingBuilderExtensions.AddXPikeLogging() or
        /// ILoggingBuilderExtensions.UseXPikeLogging() from within a call to
        /// IWebHostBuilder.ConfigureLogging() in Program.cs.
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
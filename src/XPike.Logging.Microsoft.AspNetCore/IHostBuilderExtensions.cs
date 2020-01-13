using Microsoft.Extensions.Hosting;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public static class IHostBuilderExtensions
    {
        /// <summary>
        /// Adds XPike Logging as a provider to Microsoft Extensions Logging.
        /// 
        /// NOTE: You must also call IApplicationBuilder.AddXPikeLogging() or
        /// IApplicationBuilder.UseXPikeLogging() in Startup.Configure().
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder AddXPikeLogging(this IHostBuilder builder) =>
            builder.ConfigureLogging(factory => factory.AddXPikeLogging());

        /// <summary>
        /// Adds Microsoft Extensions Logging as a provider to XPike Logging.
        ///
        /// NOTE: When using XPike Dependency Injection you should call
        /// IDependencyCollection.AddXPikeMicrosoftLogging() in
        /// Startup.ConfigureServices() instead.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder AddXPikeMicrosoftLogging(this IHostBuilder builder) =>
            builder.ConfigureServices((context, services) => services.AddXPikeMicrosoftLogging());

        /// <summary>
        /// Adds XPike Logging as the only provider for Microsoft Extensions Logging
        /// after removing any previously configured providers.
        /// 
        /// NOTE: You must also call IApplicationBuilder.AddXPikeLogging() or
        /// IApplicationBuilder.UseXPikeLogging() in Startup.Configure().
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseXPikeLogging(this IHostBuilder builder) =>
            builder.ConfigureLogging(factory => factory.UseXPikeLogging());

        /// <summary>
        /// Adds Microsoft Extensions Logging as the only provider for XPike Logging
        /// after removing any previously configured providers.
        /// 
        /// NOTE: When using XPike Dependency Injection you should call
        /// IDependencyCollection.UseMicrosoftLoggingForXPike() in
        /// Startup.ConfigureServices() instead.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseMicrosoftLoggingForXPike(this IHostBuilder builder) =>
            builder.ConfigureServices((context, services) => services.UseMicrosoftLoggingForXPike());
    }
}
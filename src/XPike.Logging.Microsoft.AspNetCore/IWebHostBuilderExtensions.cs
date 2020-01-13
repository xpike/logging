using Microsoft.AspNetCore.Hosting;

namespace XPike.Logging.Microsoft.AspNetCore
{
    /// <summary>
    /// NOTE: For .NET Core 3 or higher, use the IHostBuilder extension methods instead.
    /// </summary>
    public static class IWebHostBuilderExtensions
    {
        /// <summary>
        /// Adds XPike Logging as a provider to Microsoft Extensions Logging.
        /// 
        /// NOTE: You must also call IApplicationBuilder.AddXPikeLogging() or
        /// IApplicationBuilder.UseXPikeLogging() in Startup.Configure().
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IWebHostBuilder AddXPikeLogging(this IWebHostBuilder builder) =>
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
        public static IWebHostBuilder AddXPikeMicrosoftLogging(this IWebHostBuilder builder) =>
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
        public static IWebHostBuilder UseXPikeLogging(this IWebHostBuilder builder) =>
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
        public static IWebHostBuilder UseMicrosoftLoggingForXPike(this IWebHostBuilder builder) =>
            builder.ConfigureServices((context, services) => services.UseMicrosoftLoggingForXPike());
    }
}
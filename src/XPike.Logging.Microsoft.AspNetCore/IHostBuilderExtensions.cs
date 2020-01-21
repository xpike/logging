using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace XPike.Logging.Microsoft.AspNetCore
{
    /// <summary>
    /// NOTE: For .NET Core 2.2 and lower, use the IWebHostBuilder extension methods instead.
    /// </summary>
    public static class IHostBuilderExtensions
    {
        /// <summary>
        /// Adds XPike Logging as a provider to Microsoft Extensions Logging.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder AddXPikeLogging(this IHostBuilder builder, Action<ILoggingBuilder> logBuilder = null) =>
            builder.ConfigureLogging(factory =>
                {
                    factory.AddXPikeLogging();
                    logBuilder?.Invoke(factory);
                })
                .ConfigureServices((context, collection) =>
                {
                    collection.AddXPikeLogging();
                    collection.AddSingleton<IStartupFilter, StartupFilter>(_ => new StartupFilter(true));
                });

        /// <summary>
        /// Adds Microsoft Extensions Logging as a provider to XPike Logging.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder AddXPikeMicrosoftLogging(this IHostBuilder builder) =>
            builder.ConfigureServices((context, services) =>
            {
                services.AddXPikeMicrosoftLogging();
                services.AddSingleton<IStartupFilter, StartupFilter>(_ => new StartupFilter(false));
            });

        /// <summary>
        /// Adds XPike Logging as the only provider for Microsoft Extensions Logging
        /// after removing any previously configured providers.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseXPikeLogging(this IHostBuilder builder, Action<ILoggingBuilder> logBuilder = null) =>
            builder.ConfigureLogging(factory =>
                {
                    factory.UseXPikeLogging();
                    logBuilder?.Invoke(factory);
                })
                .ConfigureServices((context, collection) =>
                {
                    collection.AddXPikeLogging();
                    collection.AddSingleton<IStartupFilter, StartupFilter>(_ => new StartupFilter(true));
                });

        /// <summary>
        /// Adds Microsoft Extensions Logging as the only provider for XPike Logging
        /// after removing any previously configured providers.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseMicrosoftLoggingForXPike(this IHostBuilder builder) =>
            builder.ConfigureServices((context, services) =>
            {
                services.UseMicrosoftLoggingForXPike();
                services.AddSingleton<IStartupFilter, StartupFilter>(_ => new StartupFilter(false));
            });
    }
}
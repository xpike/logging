using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using XPike.IoC;
using XPike.IoC.Microsoft;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the XPike Microsoft Logging package to the DI container after removing
        /// all other XPike Logging providers.
        ///
        /// NOTE: When using XPike Dependency Injection you should call
        /// IDependencyCollection.UseMicrosoftLoggingForXPike() instead, eg:
        /// services.AddXPikeDependencyInjection().UseMicrosoftLoggingForXPike()
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseMicrosoftLoggingForXPike(this IServiceCollection services) =>
            services.AddXPikeMicrosoftLogging()
                .RemoveAll<ILogProvider>()
                .AddSingleton<ILogProvider, MicrosoftLogProvider>();

        /// <summary>
        /// Adds the XPike Logging package to the DI container.
        ///
        /// NOTE: When using XPike Dependency Injection you should call
        /// IDependencyCollection.AddXPikeLogging() instead, eg:
        /// services.AddXPikeDependencyInjection().AddXPikeLogging()
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddXPikeLogging(this IServiceCollection services)
        {
            new MicrosoftDependencyCollection(services).LoadPackage(new XPike.Logging.Package());
            return services;
        }

        /// <summary>
        /// Adds the XPike Microsoft Logging package to the DI container.
        ///
        /// NOTE: When using XPike Dependency Injection you should call
        /// IDependencyCollection.AddXPikeLogging() instead, eg:
        /// services.AddXPikeDependencyInjection().AddXPikeMicrosoftLogging()
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddXPikeMicrosoftLogging(this IServiceCollection services)
        {
            new MicrosoftDependencyCollection(services).LoadPackage(new XPike.Logging.Microsoft.Package());
            return services;
        }
    }
}
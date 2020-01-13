using Microsoft.Extensions.DependencyInjection;
using XPike.IoC.Microsoft;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the XPike Microsoft Logging package to the DI container after removing
        /// all other XPike Logging providers.
        ///
        /// NOTE: When using XPike Dependency Injection, you should call
        /// IDependencyCollection.UseMicrosoftLoggingForXPike() instead.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseMicrosoftLoggingForXPike(this IServiceCollection services)
        {
            new MicrosoftDependencyCollection(services).UseMicrosoftLoggingForXPike();
            return services;
        }

        /// <summary>
        /// Adds the XPike Logging package to the DI container.
        /// 
        /// NOTE: When using XPike Dependency Injection, you should call
        /// IDependencyCollection.AddXPikeLogging() instead.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddXPikeLogging(this IServiceCollection services)
        {
            new MicrosoftDependencyCollection(services).AddXPikeLogging();
            return services;
        }

        /// <summary>
        /// Adds the XPike Microsoft Logging package to the DI container.
        /// 
        /// NOTE: When using XPike Dependency Injection, you should call
        /// IDependencyCollection.AddXPikeMicrosoftLogging() instead.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddXPikeMicrosoftLogging(this IServiceCollection services)
        {
            new MicrosoftDependencyCollection(services).AddXPikeMicrosoftLogging();
            return services;
        }
    }
}
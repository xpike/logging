using XPike.IoC;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public static class IDependencyCollectionExtensions
    {
        /// <summary>
        /// Adds the XPike Logging package to the DI container.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IDependencyCollection AddXPikeLogging(this IDependencyCollection collection) =>
            collection.LoadPackage(new XPike.Logging.Package());

        /// <summary>
        /// Adds the XPike Microsoft Logging package to the DI container.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IDependencyCollection AddXPikeMicrosoftLogging(this IDependencyCollection collection) =>
            collection.LoadPackage(new XPike.Logging.Microsoft.Package());

        /// <summary>
        /// Adds the XPike Microsoft Logging package to the DI container after removing
        /// all other XPike Logging providers.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IDependencyCollection UseMicrosoftLoggingForXPike(this IDependencyCollection services)
        {
            services.AddXPikeMicrosoftLogging();

            services.ResetCollection<ILogProvider>();
            services.AddSingletonToCollection<ILogProvider, MicrosoftLogProvider>();

            return services;
        }
    }
}
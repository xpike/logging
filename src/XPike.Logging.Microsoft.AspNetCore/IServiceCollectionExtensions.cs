using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using XPike.IoC;
using XPike.IoC.Microsoft;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection UseMicrosoftLoggingForXPike(this IServiceCollection services) =>
            services.RemoveAll<ILogProvider>()
                .AddSingleton<ILogProvider, MicrosoftLogProvider>();

        public static IServiceCollection AddXPikeLogging(this IServiceCollection services)
        {
            new MicrosoftDependencyCollection(services).LoadPackage(new XPike.Logging.Package());
            return services;
        }
    }
}
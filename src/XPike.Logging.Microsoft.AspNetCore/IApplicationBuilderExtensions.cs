using Microsoft.AspNetCore.Builder;
using XPike.IoC.Microsoft;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseXPikeLogging(this IApplicationBuilder builder)
        {
            new MicrosoftDependencyProvider(builder.ApplicationServices).UseXPikeLogging();
            return builder;
        }
    }
}
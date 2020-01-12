using Microsoft.AspNetCore.Builder;
using XPike.IoC.Microsoft;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Enables XPike Logging.
        ///
        /// NOTE: When using XPike Dependency Injection, you should call
        /// IApplicationBuilder.UseXPikeDependencyInjection().UseXPikeLogging() instead.
        ///
        /// NOTE: You must also call ILoggingBuilder.AddXPikeLogging() or
        /// ILoggingBuilder.UseXPikeLogging() from within a call to
        /// IWebHostBuilder.ConfigureLogging() in Program.cs.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseXPikeLogging(this IApplicationBuilder builder)
        {
            new MicrosoftDependencyProvider(builder.ApplicationServices).UseXPikeLogging();
            return builder;
        }
    }
}
using Microsoft.AspNetCore.Builder;
using XPike.IoC.Microsoft;

namespace XPike.Logging.Microsoft.AspNetCore
{
    /// <summary>
    /// NOTE: When using XPike Dependency Injection you should use the
    /// IDependencyProvider extension methods instead.
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Enables XPike Logging.
        ///
        /// NOTE: You must also call either IHostBuilder.AddXPikeLogging() or
        /// IHostBuilder.UseXPikeLogging() in Program.cs.
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
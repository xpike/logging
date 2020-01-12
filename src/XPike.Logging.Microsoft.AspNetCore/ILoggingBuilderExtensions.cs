using Microsoft.Extensions.Logging;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public static class ILoggingBuilderExtensions
    {
        /// <summary>
        /// Attaches XPike as a provider for Microsoft.Extensions.Logging so that it receives
        /// log entries such as those recorded using ILogger&lt;T&gt;.
        ///
        /// NOTE: You must also call IDependencyProvider.UseXPikeLogging() in Startup.Configure(),
        /// or set XPikeLoggerProvider.LogService to an instance of XPike's ILogService to be used.
        /// </summary>
        /// <param name="builder">The logging builder to attach XPike to as a provider.</param>
        /// <returns>The logging builder.</returns>
        public static ILoggingBuilder AddXPikeLogging(this ILoggingBuilder builder) =>
            builder.AddProvider(new XPikeLoggerProvider());

        /// <summary>
        /// Clears the list of providers and attaches XPike as the only provider for
        /// Microsoft.Extensions.Logging so that it receives log entries such as those
        /// recorded using ILogger&lt;T&gt;.
        /// 
        /// NOTE: You must also call IDependencyProvider.UseXPikeLogging() in Startup.Configure(),
        /// or set XPikeLoggerProvider.LogService to an instance of XPike's ILogService to be used.
        /// </summary>
        /// <param name="builder">The logging builder to attach XPike to as a provider.</param>
        /// <returns>The logging builder.</returns>
        public static ILoggingBuilder UseXPikeLogging(this ILoggingBuilder builder) =>
            builder.ClearProviders()
                .AddXPikeLogging();
    }
}
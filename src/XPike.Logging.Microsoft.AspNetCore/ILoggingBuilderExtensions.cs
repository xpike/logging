using Microsoft.Extensions.Logging;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public static class ILoggingBuilderExtensions
    {
        public static ILoggingBuilder AddXPikeLogging(this ILoggingBuilder builder) =>
            builder.AddProvider(new XPikeLoggerProvider());

        public static ILoggingBuilder UseXPikeLogging(this ILoggingBuilder builder) =>
            builder.ClearProviders()
                .AddXPikeLogging();
    }
}
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public class StartupFilter
        : IStartupFilter
    {
        private readonly bool _useXPikeLogging;

        public StartupFilter(bool useXPikeLogging)
        {
            _useXPikeLogging = useXPikeLogging;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) =>
            builder =>
            {
                if (_useXPikeLogging)
                    builder.UseXPikeLogging();

                builder.UseXPikeRequestLoggingMiddleware();

                next(builder);
            };
    }
}
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public class StartupFilter
        : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) =>
            builder =>
            {
                builder.UseXPikeLogging();
                next(builder);
            };
    }
}
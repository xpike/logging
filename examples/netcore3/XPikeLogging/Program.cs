using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using XPike.Configuration.Microsoft.AspNetCore;
using XPike.Logging.Microsoft.AspNetCore;

namespace XPikeLogging
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(builder => { builder.UseXPikeLogging(); }) // NOTE: Call AddXPikeLogging() to preserve any configured NetCore providers.
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        //.ConfigureAppConfiguration(builder =>
                        //    builder.ConfigureXPikeConfiguration(xpike => { }))
                        .UseStartup<Startup>()
                        .AddXPikeConfiguration(builder => { });
                });
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XPike.IoC;
using XPike.IoC.Microsoft.AspNetCore;
using XPike.Logging.Microsoft.AspNetCore;
using XPike.Settings.AspNetCore;

namespace XPikeLogging
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddXPikeSettings();
            //services.AddXPikeLogging();

            services.AddXPikeDependencyInjection()
                .AddXPikeLogging()
                //.AddXPikeConfiguration(xpike => xpike.AddProvider(new MicrosoftConfigurationProvider(Configuration)))
                //.LoadPackage(new XPike.Logging.Package())
                //.LoadPackage(new XPike.Settings.Package())
                .LoadPackage(new Example.Library.Package());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseXPikeDependencyInjection()
                .UseXPikeLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

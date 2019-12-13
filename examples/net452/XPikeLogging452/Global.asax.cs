using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using XPike.Configuration;
using XPike.IoC;
using XPike.IoC.SimpleInjector;

namespace XPikeLogging452
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Configure the xPike DependencyProvider...
            IDependencyProvider dependencyProvider = ConfigureServices();

            // Tell WebAPI to use xPike to resolve dependencies...
            GlobalConfiguration.Configuration.UseXPikeDependencyInjection(dependencyProvider);
        }

        private IDependencyProvider ConfigureServices()
        {
            IDependencyCollection dependencyCollection =
                new SimpleInjectorDependencyCollection()
                    .Configure(GlobalConfiguration.Configuration);

            dependencyCollection.LoadPackage(new XPike.Configuration.System.Package())
                .LoadPackage(new XPike.Logging.Package())
                .LoadPackage(new Example.Library.Package());

            // Register application dependencies
            // dependencyCollection.RegisterSingleton<ILogger, Logger>();

            return dependencyCollection.BuildDependencyProvider();
        }
    }
}

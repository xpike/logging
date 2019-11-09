using System;
using XPike.IoC;

namespace XPike.Logging.Azure
{
    public class Package
        : IDependencyPackage
    {
        public void RegisterPackage(IDependencyCollection dependencyCollection)
        {
            dependencyCollection.LoadPackage(new XPike.Logging.Package());

            dependencyCollection.RegisterSingleton<IAzureLogProvider, AzureLogProvider>();

            dependencyCollection.AddSingletonToCollection<ILogProvider, IAzureLogProvider>(services =>
                services.ResolveDependency<IAzureLogProvider>());
        }
    }
}

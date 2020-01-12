using System;
using XPike.IoC;

namespace XPike.Logging.Microsoft
{
    public class Package
        : IDependencyPackage
    {
        public void RegisterPackage(IDependencyCollection dependencyCollection)
        {
            dependencyCollection.LoadPackage(new XPike.Logging.Package());

            dependencyCollection.RegisterSingleton<IMicrosoftLogProvider, MicrosoftLogProvider>();

            //dependencyCollection.ResetCollection<ILogProvider>();
            dependencyCollection.AddSingletonToCollection<ILogProvider, IMicrosoftLogProvider>(services =>
                services.ResolveDependency<IMicrosoftLogProvider>());
        }
    }
}

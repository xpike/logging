using XPike.IoC;

namespace Example.Library
{
    public class Package
        : IDependencyPackage
    {
        public void RegisterPackage(IDependencyCollection dependencyCollection)
        {
            dependencyCollection.RegisterSingleton<ITestModule, TestModule>();
        }
    }
}
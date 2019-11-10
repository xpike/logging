using XPike.IoC;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public static class IDependencyCollectionExtensions
    {
        public static IDependencyCollection AddXPikeLogging(this IDependencyCollection collection) =>
            collection.LoadPackage(new XPike.Logging.Package());
    }
}
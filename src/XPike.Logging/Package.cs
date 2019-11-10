using System;
using XPike.IoC;
using XPike.Logging.Console;
using XPike.Logging.Debug;
using XPike.Logging.File;

namespace XPike.Logging
{
    /// <summary>
    /// Core logging package
    /// Implements the <see cref="XPike.IoC.IDependencyPackage" />
    /// </summary>
    /// <seealso cref="XPike.IoC.IDependencyPackage" />
    public class Package
        : IDependencyPackage
    {
        /// <summary>
        /// Registers the package with the provided <see cref="T:XPike.IoC.IDependencyCollection" />.
        /// </summary>
        /// <param name="dependencyCollection">The dependency collection in which to perform the registrations.</param>
        public void RegisterPackage(IDependencyCollection dependencyCollection)
        {
            if (dependencyCollection == null)
                throw new ArgumentNullException(nameof(dependencyCollection));

            dependencyCollection.LoadPackage(new XPike.Settings.Package());

            dependencyCollection.RegisterSingleton<IConsoleLogProvider, ConsoleLogProvider>();
            dependencyCollection.RegisterSingleton<IDebugLogProvider, DebugLogProvider>();
            dependencyCollection.RegisterSingleton<IFileLogProvider, FileLogProvider>();

            dependencyCollection.AddSingletonToCollection<ILogProvider, IConsoleLogProvider>(services =>
                services.ResolveDependency<IConsoleLogProvider>());

            dependencyCollection.AddSingletonToCollection<ILogProvider, IDebugLogProvider>(services =>
                services.ResolveDependency<IDebugLogProvider>());

            dependencyCollection.RegisterSingleton<ILogService, LogService>();
            dependencyCollection.RegisterSingletonFallback(typeof(ILog<>), typeof(LogWriter<>));

            dependencyCollection.RegisterSingleton<ITraceContextProvider, TraceContextProvider>();
            dependencyCollection.RegisterSingleton<ITraceContextAccessor, TraceContextAccessor>();
            
            dependencyCollection.RegisterScoped<ITraceContext>(services =>
                services.ResolveDependency<ITraceContextAccessor>().TraceContext);
        }
    }
}
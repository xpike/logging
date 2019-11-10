using XPike.IoC;
using XPike.Logging.Console;
using XPike.Logging.Debug;
using XPike.Logging.File;

namespace XPike.Logging
{
    public class Package
        : IDependencyPackage
    {
        public void RegisterPackage(IDependencyCollection dependencyCollection)
        {
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
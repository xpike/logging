using System;
using System.Threading.Tasks;
using XPike.Configuration;
using XPike.Logging.Console;
using cons = System.Diagnostics.Debug;

namespace XPike.Logging.Debug
{
    public class DebugLogProvider
        : ConsoleLogProvider,
          IDebugLogProvider
    {
        private readonly IConfigurationService _configService;
        private IConfig<DebugLogConfig> _config;

        protected new IConfig<DebugLogConfig> Config
        {
            get => _config;
            set
            {
                _config = value;
                base.Config = new Config<ConsoleLogConfig>(value.ConfigurationKey, value.CurrentValue, _configService);
            }
        }

        public DebugLogProvider(IConfigurationService configService, IConfigManager<DebugLogConfig> configManager)
        {
            _configService = configService;

            Config = configManager.GetConfigOrDefault(new DebugLogConfig
            {
                Enabled = false,
                ShowMetadata = false,
                ShowStackTraces = false
            });
        }

        public override async Task<bool> WriteAsync(LogEvent logEvent)
        {
            var captured = false;

            try
            {
                if (!Config.CurrentValue.Enabled || !System.Diagnostics.Debugger.IsAttached)
                    return true;

                var message = ConstructMessage(logEvent);

                await Semaphore.WaitAsync().ConfigureAwait(false);
                captured = true;

                cons.WriteLine(message);

                return true;
            }
            catch (Exception ex)
            {
                cons.WriteLine($"*** Debug Logging Provider threw an Exception: {ex.Message} ({ex.GetType().Name})\r\n{ex}");
                return false;
            }
            finally
            {
                if (captured)
                    Semaphore.Release();
            }
        }
    }
}
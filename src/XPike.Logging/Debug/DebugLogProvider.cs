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
        private DebugLogSettings _settings;

        protected new DebugLogSettings Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                base.Settings = value;
            }
        }

        public DebugLogProvider(IConfigurationService configService)
            : base(configService)
        {
            base.Settings = Settings = configService.GetValueOrDefault($"XPike.Logging::{nameof(DebugLogSettings)}",
                new DebugLogSettings
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
                if (!Settings.Enabled || !System.Diagnostics.Debugger.IsAttached)
                    return true;

                var message = ConstructMessage(logEvent);

                await Semaphore.WaitAsync();
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
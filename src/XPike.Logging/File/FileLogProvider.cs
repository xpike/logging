using System;
using System.Threading;
using System.Threading.Tasks;
using XPike.Configuration;
using XPike.Logging.Console;

namespace XPike.Logging.File
{
    public class FileLogProvider
        : ConsoleLogProvider,
          IFileLogProvider,
          IDisposable
    {
        private readonly IConfigurationService _configService;

        private SemaphoreSlim _semaphore;
        private IConfig<FileLogConfig> _config;

        protected new IConfig<FileLogConfig> Config
        {
            get => _config;
            set
            {
                _config = value;
                base.Config = new Config<ConsoleLogConfig>(value.ConfigurationKey, value.CurrentValue, _configService);
            }
        }

        public FileLogProvider(IConfigurationService configService, IConfigManager<FileLogConfig> configManager)
        {
            _configService = configService;

            _semaphore = new SemaphoreSlim(1);
            _config = configManager.GetConfigOrDefault(new FileLogConfig
            {
                Enabled = false
            });
        }

        public override async Task<bool> WriteAsync(LogEvent logEvent)
        {
            var captured = false;

            try
            {
                if (!_config.CurrentValue.Enabled)
                    return true;

                var message = ConstructMessage(logEvent);

                await _semaphore.WaitAsync().ConfigureAwait(false);
                captured = true;

                System.IO.File.AppendAllText(_config.CurrentValue.Path, message);

                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"*** File Logging Provider threw an Exception: {ex.Message} ({ex.GetType().Name})\r\n{ex}");
                return false;
            }
            finally
            {
                if (captured)
                    _semaphore.Release();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _semaphore?.Dispose();
                _semaphore = null;
            }
        }
    }
}
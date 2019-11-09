using System;
using System.Threading;
using System.Threading.Tasks;
using XPike.Configuration;
using XPike.Logging.Console;

namespace XPike.Logging.File
{
    public class FileLogProvider
        : ConsoleLogProvider,
          IFileLogProvider
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly FileLogSettings _settings;

        public FileLogProvider(IConfigurationService configService)
            : base(configService)
        {
            _semaphore = new SemaphoreSlim(1);
            _settings = configService.GetValueOrDefault($"XPike.Logging::{nameof(FileLogSettings)}",
                new FileLogSettings
                {
                    Enabled = false
                });
        }

        public override async Task<bool> WriteAsync(LogEvent logEvent)
        {
            var captured = false;

            try
            {
                if (!_settings.Enabled)
                    return true;

                var message = ConstructMessage(logEvent);

                await _semaphore.WaitAsync();
                captured = true;

                System.IO.File.AppendAllText(_settings.Path, message);

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
    }
}
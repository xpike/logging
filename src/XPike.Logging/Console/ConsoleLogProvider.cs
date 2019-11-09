﻿using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XPike.Configuration;
using cons = System.Console;

namespace XPike.Logging.Console
{
    public class ConsoleLogProvider
        : IConsoleLogProvider
    {
        protected static SemaphoreSlim Semaphore { get; }

        private readonly ConsoleLogSettings _settings;

        static ConsoleLogProvider()
        {
            Semaphore = new SemaphoreSlim(1);
        }

        public ConsoleLogProvider(IConfigurationService configService)
        {
            _settings = configService.GetValueOrDefault($"XPike.Logging::{nameof(ConsoleLogSettings)}",
                new ConsoleLogSettings
                {
                    ShowMetadata = true,
                    ShowStackTraces = true,
                    Enabled = false
                });
        }

        protected virtual string ConstructMessage(LogEvent logEvent)
        {
            var sb = new StringBuilder();
            
            sb.Append($"[{logEvent.LogLevel}] {logEvent.Timestamp:M/d/yyyy hh:mm:ss.zz tt} - {logEvent.Message} ({logEvent.Category}::{logEvent.Location})");

            if (logEvent.Exception != null)
            {
                sb.Append($"\r\n\tException Details: {logEvent.Exception.Message} ({logEvent.Exception.GetType().Name})");

                if (_settings.ShowStackTraces)
                    sb.Append($"\r\nStack Trace:\r\n{logEvent.Exception}");
            }

            if (logEvent.Metadata?.Any() ?? false && _settings.ShowMetadata)
            {
                sb.Append("\r\n----------------------------------------");

                foreach (var data in logEvent.Metadata)
                    sb.Append($"\r\n\t{data.Key} = {data.Value}");

                sb.Append("\r\n----------------------------------------");
            }

            return sb.ToString();
        }

        public virtual async Task<bool> WriteAsync(LogEvent logEvent)
        {
            var captured = false;

            try
            {
                if (!_settings.Enabled)
                    return true;

                var message = ConstructMessage(logEvent);

                await Semaphore.WaitAsync();
                captured = true;

                var fg = cons.ForegroundColor;
                var bg = cons.BackgroundColor;

                cons.ForegroundColor = GetColor(logEvent.LogLevel);
                cons.BackgroundColor = ConsoleColor.Black;

                cons.WriteLine(message);

                cons.ForegroundColor = fg;
                cons.BackgroundColor = bg;

                return true;
            }
            catch (Exception ex)
            {
                cons.WriteLine($"*** Console Logging Provider threw an Exception: {ex.Message} ({ex.GetType().Name})\r\n{ex}");
                return false;
            }
            finally
            {
                if (captured)
                    Semaphore.Release();
            }
        }

        protected virtual ConsoleColor GetColor(LogLevel logLevel)
        {
            switch(logLevel)
            {
                case LogLevel.Trace:
                    return ConsoleColor.DarkGray;
                case LogLevel.Debug:
                    return ConsoleColor.Gray;
                case LogLevel.Log:
                    return ConsoleColor.White;
                case LogLevel.Info:
                    return ConsoleColor.Cyan;
                case LogLevel.Warning:
                    return ConsoleColor.Yellow;
                case LogLevel.Error:
                default:
                    return ConsoleColor.Red;
            }
        }
    }
}
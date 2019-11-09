using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace XPike.Logging
{
    public interface ILog<TSource>
        where TSource : class
    {
        /// <summary>
        /// Writes a fully constructed log event.
        /// </summary>
        /// <param name="logEvent"></param>
        bool Write(LogEvent logEvent);

        /// <summary>
        /// Writes a log event.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <param name="metadata"></param>
        /// <param name="exception"></param>
        /// <param name="location"></param>
        bool Write(LogLevel logLevel,
                   string message,
                   Dictionary<string, string> metadata = null,
                   Exception exception = null,
                   [CallerMemberName] string location = null);

        /// <summary>
        /// WRites a log event with LogLevel=Trace.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="metadata"></param>
        /// <param name="location"></param>
        bool Trace(string message,
                   Dictionary<string, string> metadata = null,
                   [CallerMemberName] string location = null);

        /// <summary>
        /// Writes a log event with LogLevel=Debug.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="metadata"></param>
        /// <param name="location"></param>
        bool Debug(string message,
                   Dictionary<string, string> metadata = null,
                   [CallerMemberName] string location = null);

        /// <summary>
        /// Writes a log event with LogLevel=Log.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="metadata"></param>
        /// <param name="location"></param>
        bool Log(string message,
                 Dictionary<string, string> metadata = null,
                 [CallerMemberName] string location = null);

        /// <summary>
        /// Writes a log event with LogLevel=Info.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="metadata"></param>
        /// <param name="location"></param>
        bool Info(string message,
                  Dictionary<string, string> metadata = null,
                  [CallerMemberName] string location = null);

        /// <summary>
        /// Writes a log entry with LogLevel=Warn.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="metadata"></param>
        /// <param name="location"></param>
        bool Warn(string message,
                  Exception exception = null,
                  Dictionary<string, string> metadata = null,
                  [CallerMemberName] string location = null);

        /// <summary>
        /// Writes a log entry with LogLevel=Error.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="metadata"></param>
        /// <param name="location"></param>
        bool Error(string message,
                   Exception exception = null,
                   Dictionary<string, string> metadata = null,
                   [CallerMemberName] string location = null);
        }
    }
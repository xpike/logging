using System.Threading.Tasks;

namespace XPike.Logging
{
    /// <summary>
    /// Interface implemented by logging providers
    /// </summary>
    public interface ILogProvider
    {
        /// <summary>
        /// Write the given LogEvent to the underlying destination.
        /// </summary>
        /// <param name="logEvent"></param>
        /// <returns><c>true</c> if successful, otherwise <c>false</c></returns>
        Task<bool> WriteAsync(LogEvent logEvent);
    }
}
using System.Threading.Tasks;

namespace XPike.Logging
{
    public interface ILogProvider
    {
        Task<bool> WriteAsync(LogEvent logEvent);
    }
}
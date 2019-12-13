using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPike.Logging.Failover
{
    public class FailoverLogProvider
        : IFailoverLogProvider
    {
        private readonly IList<ILogProvider> _providers;

        /// <summary>
        /// NOTE: This is intended to be registered with DI as an instance, not using constructor injection.
        /// </summary>
        /// <param name="providers"></param>
        public FailoverLogProvider(params ILogProvider[] providers)
        {
            _providers = providers.ToList();
        }

        public async Task<bool> WriteAsync(LogEvent logEvent)
        {
            foreach(var provider in _providers)
                if (await provider.WriteAsync(logEvent).ConfigureAwait(false))
                    return true;

            return false;
        }
    }
}
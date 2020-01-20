using Microsoft.Extensions.Logging;
using System;

namespace XPike.Logging.Microsoft.AspNetCore
{
    /// <summary>
    /// Extension methods for ILoggerFactory to support xPike logging.
    /// </summary>
    public static class ILoggerFactoryExtensions
    {
        /// <summary>
        /// Adds the xPike logging framework to the ILoggerFactory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>ILoggerFactory.</returns>
        public static ILoggerFactory AddXPikeLogging(this ILoggerFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            factory.AddProvider(new XPikeLoggerProvider());
            return factory;
        }
    }
}
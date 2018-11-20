namespace Eventures.Logging
{
    using System;

    using Eventures.Data;

    using Microsoft.Extensions.Logging;

    public static class LoggerFactoryExtensions
    {
        public static ILoggerFactory AddContext(this ILoggerFactory factory, Func<string, LogLevel, bool> filter, EventuresDbContext db)
        {
            factory.AddProvider(new DbLoggerProvider(filter, db));
            return factory;
        }

        public static ILoggerFactory AddContext(this ILoggerFactory factory, LogLevel minLevel, EventuresDbContext db)
        {
            return AddContext(factory, (_, logLevel) => logLevel >= minLevel, db);
        }
    }
}

namespace Eventures.Logging
{
    using System;

    using Eventures.Data;

    using Microsoft.Extensions.Logging;

    public class DbLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;

        private readonly EventuresDbContext _db;

        public DbLoggerProvider(Func<string, LogLevel, bool> filter, EventuresDbContext db)
        {
            _filter = filter;
            _db = db;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(categoryName, _filter, _db);
        }

        public void Dispose()
        {
        }
    }
}

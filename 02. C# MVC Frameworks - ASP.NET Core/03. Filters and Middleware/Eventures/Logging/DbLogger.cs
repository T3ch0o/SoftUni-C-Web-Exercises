namespace Eventures.Logging
{
    using System;

    using Eventures.Data;
    using Eventures.Models;

    using Microsoft.Extensions.Logging;

    public class DbLogger : ILogger
    {
        private readonly string _categoryName;

        private readonly Func<string, LogLevel, bool> _filter;

        private readonly EventuresDbContext _db;

        public DbLogger(string categoryName, Func<string, LogLevel, bool> filter, EventuresDbContext db)
        {
            _categoryName = categoryName;
            _filter = filter;
            _db = db;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (logLevel == LogLevel.Error)
            {
                _db.Logs.Add(new CustomLog());
                _db.SaveChanges();
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}

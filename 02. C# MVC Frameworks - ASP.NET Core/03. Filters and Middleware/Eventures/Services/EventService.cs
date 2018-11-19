namespace Eventures.Services
{
    using System.Collections.Generic;

    using Eventures.Data;
    using Eventures.Models;
    using Eventures.Services.Interfaces;

    public class EventService : IEventService
    {
        private readonly EventuresDbContext _db;

        public EventService(EventuresDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Event> All()
        {
            return _db.Events;
        }
    }
}

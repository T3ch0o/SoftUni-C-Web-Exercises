namespace Eventures.Services
{
    using System.Collections.Generic;

    using Eventures.Areas.Event.ViewModels;
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

        public void Create(EventViewModel model)
        {
            Event eventureEvent = new Event
            {
                Name = model.Name,
                Place = model.Place,
                Start = model.Start,
                End = model.End,
                TotalTickets = model.TotalTickets,
                TicketPrice = model.TicketPrice
            };

            _db.Events.Add(eventureEvent);
            _db.SaveChanges();
        }
    }
}

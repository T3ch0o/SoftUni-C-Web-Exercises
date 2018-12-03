namespace Eventures.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Eventures.Areas.Event.ViewModels;
    using Eventures.Areas.Events.ViewModels;
    using Eventures.Data;
    using Eventures.Models;
    using Eventures.Services.Interfaces;

    using Microsoft.EntityFrameworkCore;

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

        public List<MyOrderedEventsViewModel> GetOrderedEvents(string userId)
        {
            List<MyOrderedEventsViewModel> myEvents = new List<MyOrderedEventsViewModel>();

            foreach (Order order in _db.Orders.Where(order => order.CustomerId == userId))
            {
                Event currentEvent = _db.Events.FirstOrDefault(e => e.Id == order.EventId);

                myEvents.Add(new MyOrderedEventsViewModel
                {
                    Name = currentEvent?.Name,
                    Start = currentEvent.Start,
                    End = currentEvent.End,
                    TicketsCount = order.TicketsCount
                });
            }

            return myEvents;
        }
    }
}

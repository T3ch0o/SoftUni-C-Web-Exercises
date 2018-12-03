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

            foreach (Order order in _db.Orders.Where(order => order.CustomerId == userId).Include(o => o.Event))
            {

                myEvents.Add(new MyOrderedEventsViewModel
                {
                    Name = order.Event.Name,
                    Start = order.Event.Start,
                    End = order.Event.End,
                    TicketsCount = order.TicketsCount
                });
            }

            return myEvents;
        }
    }
}

namespace Eventures.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Eventures.Areas.Event.ViewModels;
    using Eventures.Areas.Events.ViewModels;
    using Eventures.Data;
    using Eventures.Models;
    using Eventures.Services.Interfaces;

    using Microsoft.EntityFrameworkCore;

    public class EventService : IEventService
    {
        private readonly EventuresDbContext _db;

        private readonly IMapper _mapper;

        public EventService(EventuresDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public IEnumerable<Event> All()
        {
            return _db.Events.Where(e => e.TotalTickets != 0);
        }

        public void Create(EventViewModel model)
        {
            Event eventureEvent = _mapper.Map<Event>(model);

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

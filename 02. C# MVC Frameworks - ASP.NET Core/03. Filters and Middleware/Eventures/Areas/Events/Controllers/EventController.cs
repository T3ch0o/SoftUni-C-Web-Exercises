namespace Eventures.Areas.Events.Controllers
{
    using System.Collections.Generic;

    using Eventures.Areas.Event.ViewModels;
    using Eventures.Models;
    using Eventures.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        public IActionResult Index()
        {
            IEnumerable<Event> events = _eventService.All();
            List<EventViewModel> eventViewModels = new List<EventViewModel>();

            foreach (Event @event in events)
            {
                eventViewModels.Add(new EventViewModel
                {
                    Name = @event.Name,
                    Place = @event.Place,
                    Start = @event.Start,
                    End = @event.End
                });
            }

            return View(eventViewModels);
        }
    }
}
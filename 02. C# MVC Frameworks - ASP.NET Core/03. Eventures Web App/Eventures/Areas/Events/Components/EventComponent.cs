namespace Eventures.Areas.Events.Components
{
    using System.Collections.Generic;

    using AutoMapper;

    using Eventures.Areas.Event.ViewModels;
    using Eventures.Models;
    using Eventures.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    using X.PagedList;

    public class EventComponent : ViewComponent
    {
        private readonly IEventService _eventService;

        private readonly IMapper _mapper;

        public EventComponent(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        public IViewComponentResult Invoke()
        {
            IEnumerable<Event> events = _eventService.All();
            List<EventViewModel> eventViewModels = new List<EventViewModel>();

            foreach (Event @event in events)
            {
                eventViewModels.Add(_mapper.Map<EventViewModel>(@event));
            }

            IPagedList<EventViewModel> pagedViewModels = eventViewModels.ToPagedList();

            return View(eventViewModels);
        }
    }
}

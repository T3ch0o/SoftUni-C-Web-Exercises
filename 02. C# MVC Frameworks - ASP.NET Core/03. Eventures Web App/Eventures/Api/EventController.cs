namespace Eventures.Api
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Eventures.Areas.Event.ViewModels;
    using Eventures.Models;
    using Eventures.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    using Sieve.Models;
    using Sieve.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        private readonly IMapper _mapper;

        private readonly SieveProcessor _sieveProcessor;

        public EventController(IEventService eventService, IMapper mapper, SieveProcessor sieveProcessor)
        {
            _eventService = eventService;
            _mapper = mapper;
            _sieveProcessor = sieveProcessor;
        }

        [HttpGet]
        public object GetEvents(SieveModel sieveModel)
        {
            IEnumerable<Event> events = _eventService.All();
            List<EventViewModel> eventViewModels = new List<EventViewModel>();

            foreach (Event @event in events)
            {
                eventViewModels.Add(_mapper.Map<EventViewModel>(@event));
            }

            IQueryable<Event> filteredData = _sieveProcessor.Apply(sieveModel, _eventService.All());

            return filteredData;
        }
    }
}
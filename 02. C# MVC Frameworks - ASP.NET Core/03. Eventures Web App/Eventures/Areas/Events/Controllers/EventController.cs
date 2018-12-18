namespace Eventures.Areas.Events.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;

    using AutoMapper;

    using Eventures.Areas.Event.ViewModels;
    using Eventures.Areas.Events.ViewModels;
    using Eventures.Filters;
    using Eventures.Models;
    using Eventures.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using X.PagedList;

    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        private readonly IOrderService _orderService;

        private readonly IMapper _mapper;

        public EventController(IEventService eventService, IOrderService orderService, IMapper mapper)
        {
            _eventService = eventService;
            _orderService = orderService;
            _mapper = mapper;
        }

        [ServiceFilter(typeof(LogUserActivityActionFilter))]
        public IActionResult Index(int? page)
        {
            IEnumerable<Event> events = _eventService.GetAllWithTicketPrice();
            List<EventViewModel> eventViewModels = new List<EventViewModel>();

            foreach (Event @event in events)
            {
                eventViewModels.Add(_mapper.Map<EventViewModel>(@event));
            }

            int nextPage = page ?? 1;

            IPagedList<EventViewModel> pagedViewModels = eventViewModels.ToPagedList(nextPage, 5);

            return View(pagedViewModels);
        }

        [HttpPost]
        public IActionResult Index(OrderTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                bool isCreated = _orderService.CreateOrder(model, userId);

                return isCreated ? RedirectToAction("MyEvents") : RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult MyEvents()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<MyOrderedEventsViewModel> myEvents = _eventService.GetOrderedEvents(userId);

            return View(myEvents);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(EventViewModel model)
        {
            if (ModelState.IsValid)
            {
                _eventService.Create(model);

                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
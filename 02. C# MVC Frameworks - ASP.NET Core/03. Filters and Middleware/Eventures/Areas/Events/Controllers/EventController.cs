namespace Eventures.Areas.Events.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Security.Claims;

    using Eventures.Areas.Event.ViewModels;
    using Eventures.Areas.Events.ViewModels;
    using Eventures.Filters;
    using Eventures.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        private readonly IOrderService _orderService;

        public EventController(IEventService eventService, IOrderService orderService)
        {
            _eventService = eventService;
            _orderService = orderService;
        }

        [ServiceFilter(typeof(LogUserActivityActionFilter))]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(OrderTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                _orderService.CreateOrder(model, userId);

                return RedirectToAction("MyEvents");
            }

            return View(model);
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
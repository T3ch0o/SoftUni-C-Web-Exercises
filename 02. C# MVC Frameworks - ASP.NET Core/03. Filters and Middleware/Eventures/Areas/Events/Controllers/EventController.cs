namespace Eventures.Areas.Events.Controllers
{
    using Eventures.Areas.Event.ViewModels;
    using Eventures.Filters;
    using Eventures.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [ServiceFilter(typeof(LogUserActivityActionFilter))]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize("Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize("Administrator")]
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
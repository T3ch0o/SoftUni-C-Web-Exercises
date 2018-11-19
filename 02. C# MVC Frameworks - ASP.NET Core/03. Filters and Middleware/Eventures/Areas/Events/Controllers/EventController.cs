namespace Eventures.Areas.Events.Controllers
{
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


            return View();
        }
    }
}
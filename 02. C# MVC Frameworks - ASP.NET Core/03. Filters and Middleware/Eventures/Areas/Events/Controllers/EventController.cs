namespace Eventures.Areas.Events.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class EventController : Controller
    {
        public IActionResult Index()
        {


            return View();
        }
    }
}
namespace SIS.Demo.Controllers
{
    using SIS.Framework.ActionResults.Base;
    using SIS.Framework.Controllers;

    internal class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
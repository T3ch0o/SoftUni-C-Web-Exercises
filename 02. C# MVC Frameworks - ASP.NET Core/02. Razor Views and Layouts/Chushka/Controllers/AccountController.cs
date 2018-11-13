namespace Chushka.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
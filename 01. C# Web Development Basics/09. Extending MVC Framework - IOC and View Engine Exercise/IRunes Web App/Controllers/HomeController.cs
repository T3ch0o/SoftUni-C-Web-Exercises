namespace IRunes.Controllers
{
    using System.Collections.Generic;

    using IRunes.ViewModels;

    using SIS.Framework.ActionResults.Base;
    using SIS.Framework.Attributes.Action;
    using SIS.Framework.Controllers;

    internal class HomeController : Controller
    {
        public IActionResult Index()
        {
            LoginViewModel loginViewModel = new LoginViewModel
            {
                Username = "asd",
                Password = "gfg",
                Roles = new List<string>
                {
                        "Lecture",
                        "trainer"
                }
            };
            ViewModel.Data["LoginViewModel"] = loginViewModel;

            return View();
        }
    }
}
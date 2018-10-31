namespace SIS.Demo.Controllers
{
    using System;

    using SIS.Demo.Services.Interfaces;
    using SIS.Framework.ActionResults.Base;
    using SIS.Framework.Controllers;

    internal class HomeController : Controller
    {
        private readonly IHashService _hashService;

        public HomeController(IHashService hashService)
        {
            _hashService = hashService;
        }

        public IActionResult Index(IndexViewModel viewModel)
        {
            Console.WriteLine(_hashService.Hash(viewModel.Username));

            return View();
        }
    }
}
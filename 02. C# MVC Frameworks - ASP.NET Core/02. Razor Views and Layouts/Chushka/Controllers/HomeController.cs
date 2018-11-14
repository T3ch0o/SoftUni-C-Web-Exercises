namespace Chushka.Controllers
{
    using System.Collections.Generic;

    using Chushka.Models;
    using Chushka.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(_productService.GetProducts());
            }

            return View();
        }
    }
}

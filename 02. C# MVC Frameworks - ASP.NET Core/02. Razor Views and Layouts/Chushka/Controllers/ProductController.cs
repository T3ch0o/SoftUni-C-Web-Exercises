namespace Chushka.Controllers
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    public class ProductController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}
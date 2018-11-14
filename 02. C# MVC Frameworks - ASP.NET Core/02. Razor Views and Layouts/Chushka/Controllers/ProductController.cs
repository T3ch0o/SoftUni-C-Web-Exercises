namespace Chushka.Controllers
{
    using Chushka.Models.ViewModels;
    using Chushka.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public RedirectResult Create(CreateProductViewModel model)
        {
            _productService.AddProduct(model);

            return Redirect("/");
        }
    }
}
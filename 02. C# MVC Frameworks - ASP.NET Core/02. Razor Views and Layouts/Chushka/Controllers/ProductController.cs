namespace Chushka.Controllers
{
    using Chushka.Models.ViewModels;
    using Chushka.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public RedirectResult Create(CreateProductViewModel model)
        {
            _productService.AddProduct(model);

            return Redirect("/");
        }
    }
}
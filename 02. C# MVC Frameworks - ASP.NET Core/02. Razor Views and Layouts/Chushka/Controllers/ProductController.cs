namespace Chushka.Controllers
{
    using System.Security.Claims;

    using Chushka.Models;
    using Chushka.Models.ViewModels;
    using Chushka.Services;
    using Chushka.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        private readonly IOrderService _orderService;

        public ProductController(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public RedirectResult Create(ProductViewModel model)
        {
            _productService.AddProduct(model);

            return Redirect("/");
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            Product product =_productService.GetProduct(id);

            return View(product);
        }

        [Authorize]
        public RedirectResult Order(int id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            _orderService.CreateOrder(userId, id);

            return Redirect("/");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            Product product = _productService.GetProduct(id);
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SelectedFoodType = product.Type.ToString(),
                Id = product.Id
            };

            return View(productViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public RedirectResult Edit(ProductViewModel model)
        {
            _productService.EditProduct(model);

            return Redirect("/");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            Product product = _productService.GetProduct(id);
            ProductViewModel productViewModel = new ProductViewModel()
            {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    SelectedFoodType = product.Type.ToString(),
                    Id = product.Id
            };

            return View(productViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public RedirectResult Delete(ProductViewModel model)
        {
            _productService.DeleteProduct(model.Id);

            return Redirect("/");
        }
    }
}
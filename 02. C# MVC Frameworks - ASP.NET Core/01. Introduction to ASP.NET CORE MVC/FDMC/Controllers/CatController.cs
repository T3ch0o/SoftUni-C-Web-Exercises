namespace FDMC.Controllers
{
    using FDMC.Models;
    using FDMC.Models.ViewModels;
    using FDMC.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    public class CatController : Controller
    {
        private readonly ICatService _catService;

        public CatController(ICatService catService)
        {
            _catService = catService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CatViewModel model)
        {
            _catService.AddCat(model);

            return Redirect("/");
        }

        public IActionResult Details(int id)
        {
            Cat cat = _catService.GetCat(id);

            return View(cat);
        }
    }
}

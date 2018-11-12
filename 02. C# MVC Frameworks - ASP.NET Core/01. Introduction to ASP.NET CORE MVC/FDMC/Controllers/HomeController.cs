namespace FDMC.Controllers
{
    using FDMC.Models.ViewModels;
    using FDMC.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly ICatService _catService;

        public HomeController(ICatService catService)
        {
            _catService = catService;
        }

        public IActionResult Index()
        {
            AllCatsViewModel allCats = new AllCatsViewModel()
            {
                Cats = _catService.GetAllCats()
            };

            return View(allCats);
        }
    }
}

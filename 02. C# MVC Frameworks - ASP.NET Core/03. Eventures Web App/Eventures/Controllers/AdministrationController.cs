namespace Eventures.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Eventures.Models;
    using Eventures.Models.ViewModels;
    using Eventures.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using X.PagedList;

    [Authorize(Roles = "Administrator")]
    public class AdministrationController : Controller
    {
        private readonly IUserService _userService;

        private readonly UserManager<ApplicationUser> _userManager;

        public AdministrationController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Users(int? page)
        {
            IQueryable<ApplicationUser> users = _userService.GetAll(User.Identity.Name);
            List<UserViewModel> allUsers = new List<UserViewModel>();

            foreach (ApplicationUser user in users)
            {
                string role;

                if (await _userManager.IsInRoleAsync(user, "Administrator"))
                {
                    role = "Admin";
                }
                else
                {
                    role = "User";
                }

                allUsers.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    UserRole = role
                });
            }

            int nextPage = page ?? 1;

            IPagedList<UserViewModel> pagedUsers = allUsers.ToPagedList(nextPage, 5);

            return View(pagedUsers);
        }

        [HttpPost]
        public async Task<RedirectToActionResult> Users(string userData)
        {
            await _userService.ChangeUserRole(userData);

            return RedirectToAction("Users");
        }
    }
}
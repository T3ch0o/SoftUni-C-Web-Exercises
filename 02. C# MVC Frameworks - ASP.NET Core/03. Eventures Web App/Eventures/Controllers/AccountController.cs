namespace Eventures.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Eventures.Models;
    using Eventures.Models.ViewModels;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;

        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IAuthenticationSchemeProvider authenticationSchemeProvider,
                                 IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationSchemeProvider = authenticationSchemeProvider;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(model.Username);

                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                    if (signInResult.Succeeded)
                    {
                        return Redirect("/");
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid name or password");

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = _mapper.Map<ApplicationUser>(model);

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (_signInManager.UserManager.Users.Count() == 1)
                {
                    await _signInManager.UserManager.AddToRoleAsync(user, "Administrator");
                }

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return Redirect("/");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }

        public async Task<RedirectResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Redirect("/");
        }
    }
}
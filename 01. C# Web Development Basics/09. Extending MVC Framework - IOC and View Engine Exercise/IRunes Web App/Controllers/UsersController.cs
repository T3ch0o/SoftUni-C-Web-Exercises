namespace IRunes.Controllers
{
    using System;
    using System.Linq;
    using System.Net;

    using IRunes.Models;
    using IRunes.Services;
    using IRunes.Services.Interfaces;
    using IRunes.ViewModels;

    using SIS.Framework.ActionResults.Base;
    using SIS.Framework.Attributes.Action;
    using SIS.Framework.Attributes.Methods;
    using SIS.Framework.Controllers;
    using SIS.Framework.Security;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses.Interfaces;
    using SIS.WebServer.Result;

    internal class UsersController : Controller
    {
        private readonly IUsersService _userService;

        public UsersController(IUsersService usersService)
        {
            _userService = usersService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid.HasValue || !ModelState.IsValid.Value)
            {
                return RedirectToAction("/users/login");
            }

            string username = WebUtility.UrlDecode(model.Username.Trim());
            bool userExists = _userService.UserExists(username, model.Password);

            if (!userExists)
            {
                return RedirectToAction("/users/login");
            }

            SignIn(new IdentityUser{Username = username, Password = model.Password});
            return RedirectToAction("/");
        }

        public IActionResult Register()
        {
            return View();
        }

        //public IHttpResponse RegisterPost(IHttpRequest request)
        //{
        //    string username = request.FormData["username"].ToString().Trim();
        //    string email = WebUtility.UrlDecode(request.FormData["email"].ToString().Trim());
        //    string password = request.FormData["password"].ToString();
        //    string confirmPassword = request.FormData["confirmPassword"].ToString();

        //    if (string.IsNullOrWhiteSpace(username) || username.Length < 4)
        //    {
        //        return BadRequestError("Please provide valid username with length of 4 or more character.");
        //    }

        //    if (string.IsNullOrWhiteSpace(email))
        //    {
        //        return BadRequestError("Invalid email address.");
        //    }

        //    if (Db.Users.Any(u => u.Username == username))
        //    {
        //        return BadRequestError("Username already exists.");
        //    }

        //    if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        //    {
        //        return BadRequestError("Password must be 6 or more symbols.");
        //    }

        //    if (password != confirmPassword)
        //    {
        //        return BadRequestError("Password do not match.");
        //    }

        //    string hashedPassword = hashService.Hash(password);

        //    User user = new User { Username = username, Email = email, Password = hashedPassword };

        //    Db.Users.Add(user);
        //    try
        //    {
        //        Db.SaveChanges();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequestError(e.Message);
        //    }

        //    IHttpResponse response = Redirect("/home/index");

        //    SignInUser(username, request, response);

        //    return response;
        //}

        //public IHttpResponse Logout(IHttpRequest request)
        //{
        //    if (!IsAuthenticated(request))
        //    {
        //        return Redirect("/");
        //    }

        //    HttpCookie cookie = request.Cookies.GetCookie("IRunes_auth");
        //    cookie.Delete();

        //    IHttpResponse response = Redirect("/");
        //    request.Session.ClearParameters();
        //    response.Cookies.Add(cookie);

        //    return response;
        //}
    }
}
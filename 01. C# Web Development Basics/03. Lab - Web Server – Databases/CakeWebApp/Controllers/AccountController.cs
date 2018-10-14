namespace CakeWebApp.Controllers
{
    using System;

    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses.Interfaces;
    using SIS.WebServer.Result;
    using System.Linq;

    using CakeWebApp.Models;
    using CakeWebApp.Services;
    using CakeWebApp.Services.Interfaces;

    using SIS.HTTP.Cookies;

    internal class AccountController : BaseController
    {
        private const string AuthCookieHeaderName = ".auth-cakes";

        private IHashService hashService = new HashService();

        public IHttpResponse RegisterGet(IHttpRequest request)
        {
            return View("Register");
        }

        public IHttpResponse RegisterPost(IHttpRequest request)
        {
            string username = request.FormData["username"].ToString().Trim();
            string password = request.FormData["password"].ToString();
            string confirmPassword = request.FormData["confirmPassword"].ToString();

            if (string.IsNullOrWhiteSpace(username) || username.Length < 4)
            {
                return BadRequestError("Please provide valid username with length of 4 or more character.");
            }

            if (Db.Users.Any(u => u.Username == username))
            {
                return BadRequestError("Username already exists.");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return BadRequestError("Password must be 6 or more symbols.");
            }

            if (password != confirmPassword)
            {
                return BadRequestError("Password do not match.");
            }

            string hashedPassword = hashService.Hash(password);

            User user = new User
            {
                Name = username,
                Username = username,
                Password = hashedPassword
            };

            Db.Users.Add(user);
            try
            {
                Db.SaveChanges();
            }
            catch (Exception e)
            {
                return ServerError(e.Message);
            }

            return new RedirectResult("/");
        }

        public IHttpResponse LoginGet(IHttpRequest request)
        {
            return View("Login");
        }

        public IHttpResponse LoginPost(IHttpRequest request)
        {
            string username = request.FormData["username"].ToString().Trim();
            string password = request.FormData["password"].ToString();

            string hashedPassword = hashService.Hash(password);

            User user = Db.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

            if (user == null)
            {
                return BadRequestError("Invalid username or password");
            }

            IHttpResponse response = new RedirectResult("/");

            string cookie = UserCookieService.GetUserCookie(username);
            response.Cookies.Add(new HttpCookie(AuthCookieHeaderName, cookie, 7));

            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(AuthCookieHeaderName))
            {
                return new RedirectResult("/");
            }

            HttpCookie cookie = request.Cookies.GetCookie(AuthCookieHeaderName);
            cookie.Delete();
            IHttpResponse response = new RedirectResult("/");
            response.Cookies.Add(cookie);

            return response;
        }
    }
}
namespace IRunes.Controllers
{
    using System;
    using System.Linq;

    using CakeWebApp.Services;
    using CakeWebApp.Services.Interfaces;

    using IRunes.Models;

    using SIS.HTTP.Cookies;
    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses.Interfaces;
    using SIS.WebServer.Result;

    internal class UsersController : BaseController
    {
        private readonly IHashService hashService = new HashService();

        public IHttpResponse LoginGet(IHttpRequest request)
        {
            return View();
        }

        public IHttpResponse LoginPost(IHttpRequest request)
        {
            string username = request.FormData["username"].ToString().Trim();
            string password = request.FormData["password"].ToString();
            string hashedPassword = hashService.Hash(password);

            User user = Db.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

            if (user == null)
            {
                return new RedirectResult("/users/login");
            }

            IHttpResponse response = Redirect("/home/index");

            SignInUser(username, request, response);

            return response;
        }

        public IHttpResponse RegisterGet(IHttpRequest request)
        {
            return View();
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

            User user = new User { Username = username, Email = username, Password = hashedPassword };

            Db.Users.Add(user);
            try
            {
                Db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequestError(e.Message);
            }

            IHttpResponse response = Redirect("/home/index");

            SignInUser(username, request, response);

            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            if (!IsAuthenticated(request))
            {
                return Redirect("/");
            }

            HttpCookie cookie = request.Cookies.GetCookie("IRunes_auth");
            cookie.Delete();

            IHttpResponse response = Redirect("/");
            request.Session.ClearParameters();
            response.Cookies.Add(cookie);

            return response;
        }
    }
}
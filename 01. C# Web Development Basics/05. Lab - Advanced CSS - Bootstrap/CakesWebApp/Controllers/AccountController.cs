namespace CakesWebApp.Controllers
{
    using CakesWebApp.Models;
    using CakesWebApp.Services;

    public class AccountController : BaseController
    {
        private readonly IHashService hashService;

        public AccountController()
        {
            hashService = new HashService();
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return View("Register");
        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            var userName = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmPassword"].ToString();

            // Validate
            if (string.IsNullOrWhiteSpace(userName) || userName.Length < 4) return BadRequestError("Please provide valid username with length of 4 or more characters.");

            if (Db.Users.Any(x => x.Username == userName)) return BadRequestError("User with the same name already exists.");

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6) return BadRequestError("Please provide password of length 6 or more.");

            if (password != confirmPassword) return BadRequestError("Passwords do not match.");

            // Hash password
            var hashedPassword = hashService.Hash(password);

            // Create user
            User user = new User { Name = userName, Username = userName, Password = hashedPassword };
            Db.Users.Add(user);

            try
            {
                Db.SaveChanges();
            }
            catch (Exception e)
            {
                // TODO: Log error
                return ServerError(e.Message);
            }

            // TODO: Login

            // Redirect
            return new RedirectResult("/");
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            return View("Login");
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            var userName = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();

            var hashedPassword = hashService.Hash(password);

            var user = Db.Users.FirstOrDefault(x => x.Username == userName && x.Password == hashedPassword);

            if (user == null) return BadRequestError("Invalid username or password.");

            var cookieContent = UserCookieService.GetUserCookie(user.Username);

            var response = new RedirectResult("/");
            var cookie = new HttpCookie(".auth-cakes", cookieContent, 7) { HttpOnly = true };
            response.Cookies.Add(cookie);
            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(".auth-cakes")) return new RedirectResult("/");

            var cookie = request.Cookies.GetCookie(".auth-cakes");
            cookie.Delete();
            var response = new RedirectResult("/");
            response.Cookies.Add(cookie);
            return response;
        }
    }
}
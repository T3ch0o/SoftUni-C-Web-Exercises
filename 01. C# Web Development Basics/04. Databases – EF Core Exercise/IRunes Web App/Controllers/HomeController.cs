namespace IRunes.Controllers
{
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses.Interfaces;

    internal class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            if (IsAuthenticated(request))
            {
                string username = request.Session.GetParameter("username").ToString();
                ViewBag["username"] = username;

                return View("IndexLoggedIn");
            }

            return View();
        }
    }
}
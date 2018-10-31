namespace SIS.Framework.Controllers
{
    using System.Runtime.CompilerServices;

    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Interfaces;
    using SIS.Framework.Models;
    using SIS.Framework.Utilities;
    using SIS.Framework.Views;
    using SIS.HTTP.Requests.Interfaces;

    public abstract class Controller
    {
        public Model ModelState { get; } = new Model();

        public IHttpRequest Request { get; set; }

        public ViewModel ViewModel { get; set; } = new ViewModel();

        protected IViewable View([CallerMemberName] string viewName = "")
        {
            string controllerName = ControllerUtilities.GetControllerName(this);

            string viewFullQualifiedName = ControllerUtilities.GetViewFullQualifiedName(controllerName, viewName);

            View view = new View(viewFullQualifiedName, ViewModel.Data);

            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl) => new RedirectResult(redirectUrl);
    }
}
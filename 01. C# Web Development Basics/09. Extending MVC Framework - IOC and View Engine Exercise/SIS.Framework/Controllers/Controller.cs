namespace SIS.Framework.Controllers
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Interfaces;
    using SIS.Framework.Models;
    using SIS.Framework.Security;
    using SIS.Framework.Utilities;
    using SIS.Framework.Views;
    using SIS.HTTP.Requests.Interfaces;

    public abstract class Controller
    {
        public Model ModelState { get; } = new Model();

        public IHttpRequest Request { get; set; }

        public ViewModel ViewModel { get; } = new ViewModel();

        private ViewEngine ViewEngine { get; } = new ViewEngine();

        public IIdentity Identity()
        {
            if (Request.Session.ContainsParameter("auth"))
            {
                return (IIdentity)Request.Session.GetParameter("auth");
            }

            return null;
        }

        protected IViewable View([CallerMemberName] string viewName = "")
        {
            string controllerName = ControllerUtilities.GetControllerName(this);
            string viewContent = null;

            try
            {
                viewContent = ViewEngine.GetViewContent(controllerName, viewName);
            }
            catch (FileNotFoundException e)
            {
                ViewModel.Data["Error"] = e.Message;

                viewContent = ViewEngine.GetErrorContent();
            }

            string renderedHtml = ViewEngine.RenderHtml(viewContent, ViewModel.Data);

            View view = new View(renderedHtml);

            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl) => new RedirectResult(redirectUrl);

        protected void SignIn(IIdentity identity)
        {
            Request.Session.AddParameter("auth", identity);
        }

        protected void SignOut()
        {
            Request.Session.ClearParameters();
        }
    }
}
namespace SIS.Framework.Routers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using global::HTTP.WebServer.Result;

    using SIS.Framework.ActionResults.Base;
    using SIS.Framework.ActionResults.Interfaces;
    using SIS.Framework.Attributes.Methods.Base;
    using SIS.Framework.Controllers;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses.Interfaces;
    using SIS.WebServer.Api.Interfaces;
    using SIS.WebServer.Result;

    public class ControllerRouter : IHttpHandler
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            string requestMethod = request.RequestMethod.ToString();
            string controllerName = string.Empty;
            string actionName = string.Empty;

            if (request.Url == "/")
            {
                controllerName = "Home";
                actionName = "Index";
            }
            else
            {
                string[] requestUrl = request.Url
                                             .Split("/", StringSplitOptions.RemoveEmptyEntries)
                                             .Select(p => char.ToUpper(p[0]) + p.Substring(1))
                                             .ToArray();

                controllerName = requestUrl[0];
                actionName = requestUrl[1];
            }

            Controller controller = GetControllerName(controllerName, request);
            MethodInfo action = GetAction(requestMethod, controller, actionName);

            if (controller == null || action == null)
            {
                throw new NullReferenceException();
            }

            return PrepareResponse(controller, action);
        }

        private IHttpResponse PrepareResponse(Controller controller, MethodInfo action)
        {
            IActionResult actionResult = (IActionResult)action.Invoke(controller, null);
            string invocationType = actionResult.Invoke();

            if (actionResult is IViewable)
            {
                return new HtmlResult(invocationType, HttpResponseStatusCode.OK);
            }

            if (actionResult is IRedirectable)
            {
                return new RedirectResult(invocationType);
            }
            else
            {
                throw new InvalidOperationException("The view result is not supported.");
            }
        }

        private MethodInfo GetAction(string requestMethod, Controller controller, string actionName)
        {
            IEnumerable<MethodInfo> actions = GetSuitableMethods(controller, actionName).ToList();

            if (!actions.Any())
            {
                return null;
            }

            foreach (MethodInfo action in actions)
            {
                IEnumerable<HttpMethodAttribute> httpMethodAttributes = action
                                                                   .GetCustomAttributes()
                                                                   .Where(ca => ca is HttpMethodAttribute)
                                                                   .Cast<HttpMethodAttribute>()
                                                                   .ToList();

                if (!httpMethodAttributes.Any() && requestMethod.ToLower() == "get")
                {
                    return action;
                }

                foreach (HttpMethodAttribute httpMethodAttribute in httpMethodAttributes)
                {
                    if (httpMethodAttribute.IsValid(requestMethod))
                    {
                        return action;
                    }
                }
            }

            return null;
        }

        private IEnumerable<MethodInfo> GetSuitableMethods(Controller controller, string actionName)
        {
            if (controller == null)
            {
                return new MethodInfo[0];
            }

            return controller
                   .GetType()
                   .GetMethods()
                   .Where(mi => mi.Name.ToLower() == actionName.ToLower());
        }

        private Controller GetControllerName(string controllerName, IHttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(controllerName))
            {
                return null;
            }

            string fullyQualifiedControllerName = string.Format("{0}.{1}.{2}{3}, {0}",
                   MvcContext.Get.AssemblyName,
                   MvcContext.Get.ControllersFolder,
                   controllerName,
                   MvcContext.Get.ControllerSuffix);

            Type controllerType = Type.GetType(fullyQualifiedControllerName);
            Controller controller = (Controller)Activator.CreateInstance(controllerType);

            return controller;
        }
    }
}
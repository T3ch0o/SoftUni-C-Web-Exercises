namespace SIS.Framework.Routers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    using global::HTTP.WebServer.Result;

    using SIS.Framework.ActionResults.Base;
    using SIS.Framework.ActionResults.Interfaces;
    using SIS.Framework.Attributes.Methods.Base;
    using SIS.Framework.Controllers;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Extensions;
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

            if (request.Path == "/")
            {
                controllerName = "Home";
                actionName = "Index";
            }
            else
            {
                string[] requestUrl = request.Path
                                             .Split("/", StringSplitOptions.RemoveEmptyEntries)
                                             .Select(p => p.Capitalize())
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

            object[] actionParameters = MapActionParameters(action, request, controller);
            IActionResult actionResult = InvokeAction(controller, action, actionParameters);

            return PrepareResponse(actionResult);
        }

        private IHttpResponse PrepareResponse(IActionResult actionResult)
        {
            string invocationType = actionResult.Invoke();

            if (actionResult is IViewable)
            {
                return new HtmlResult(invocationType, HttpResponseStatusCode.OK);
            }

            if (actionResult is IRedirectable)
            {
                return new RedirectResult(invocationType);
            }

            throw new InvalidOperationException("The view result is not supported.");
        }

        private static IActionResult InvokeAction(Controller controller, MethodInfo action, object[] actionParameters)
        {
            return (IActionResult)action.Invoke(controller, actionParameters);
        }

        private Controller GetControllerName(string controllerName, IHttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(controllerName))
            {
                return null;
            }

            string fullyQualifiedControllerName = string.Format("{0}.{1}.{2}{3}, {0}", MvcContext.Get.AssemblyName, MvcContext.Get.ControllersFolderName, controllerName, MvcContext.Get.ControllerSuffix);

            Type controllerType = Type.GetType(fullyQualifiedControllerName);
            Controller controller = (Controller)Activator.CreateInstance(controllerType);

            return controller;
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

            return controller.GetType().GetMethods().Where(mi => mi.Name.ToLower() == actionName.ToLower());
        }

        private object[] MapActionParameters(MethodInfo action, IHttpRequest request, Controller controller)
        {
            ParameterInfo[] actionParameters = action.GetParameters();
            object[] mappedActionParameters = new object[actionParameters.Length];

            for (int i = 0; i < actionParameters.Length; i++)
            {
                ParameterInfo actionParameter = actionParameters[i];

                if (actionParameter.ParameterType.IsPrimitive || actionParameter.ParameterType == typeof(string))
                {
                    object mappedActionParameter = ProcessPrimitiveParameter(actionParameter, request);

                    if (mappedActionParameter == null)
                    {
                        break;
                    }

                    mappedActionParameters[i] = mappedActionParameter;
                }
                else
                {
                    object bindingModel = ProcessesBindingModelParameter(actionParameter, request);

                    controller.ModelState.IsValid = IsValid(bindingModel, actionParameter.ParameterType);
                }
            }

            return mappedActionParameters;
        }

        private bool? IsValid(object bindingModel, Type bindingModelType)
        {
            PropertyInfo[] properties = bindingModelType.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                IEnumerable<ValidationAttribute> propertyValidationAttributes = property
                                                                      .GetCustomAttributes()
                                                                      .Where(ca => ca is ValidationAttribute)
                                                                      .Cast<ValidationAttribute>().ToList();

                foreach (ValidationAttribute validationAttribute in propertyValidationAttributes)
                {
                    object propertyValue = property.GetValue(bindingModel);

                    if (!validationAttribute.IsValid(propertyValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private object ProcessPrimitiveParameter(ParameterInfo actionParameter, IHttpRequest request)
        {
            object value = GetParameterFromRequestData(request, actionParameter.Name);

            if (value == null)
            {
                return value;
            }

            return Convert.ChangeType(value, actionParameter.ParameterType);
        }

        private object ProcessesBindingModelParameter(ParameterInfo actionParameter, IHttpRequest request)
        {
            Type bindingModelType = actionParameter.ParameterType;

            object bindingModelInstance = Activator.CreateInstance(bindingModelType);

            PropertyInfo[] bindingModelProperties = bindingModelType.GetProperties();

            foreach (PropertyInfo bindingModelProperty in bindingModelProperties)
            {
                try
                {
                    object value = GetParameterFromRequestData(request, bindingModelProperty.Name.ToLower());
                    bindingModelProperty.SetValue(bindingModelInstance, Convert.ChangeType(value, bindingModelProperty.PropertyType));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"The property {bindingModelProperty.Name} could not be mapped.");
                }
            }

            return Convert.ChangeType(bindingModelInstance, bindingModelType);
        }

        private object GetParameterFromRequestData(IHttpRequest request, string actionParameterName)
        {
            if (request.QueryData.ContainsKey(actionParameterName))
            {
                return request.QueryData[actionParameterName];
            }

            if (request.FormData.ContainsKey(actionParameterName))
            {
                return request.FormData[actionParameterName];
            }

            return null;
        }
    }
}
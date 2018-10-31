namespace SIS.Framework.Utilities
{
    using SIS.Framework.Controllers;

    public static class ControllerUtilities
    {
        public static string GetControllerName(object controller) =>
                controller
                        .GetType()
                        .Name
                        .Replace(MvcContext.Get.ControllerSuffix, string.Empty);

        public static string GetViewFullQualifiedName(string controllerName, string viewName) =>
                $"../../../{MvcContext.Get.ViewsFolderName}/{controllerName}/{viewName}.html";
    }
}
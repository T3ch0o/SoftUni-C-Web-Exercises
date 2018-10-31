namespace SIS.Framework.Views
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class ViewEngine
    {
        private const string DisplayTemplateSuffix = "DisplayTemplate";

        private const string DisplayTemplateFolderName = "DisplayTemplates";

        private const string ErrorViewName = "_Error";

        private const string ViewExtension = "html";

        private const string ModelCollectionViewParameterPattern = @"\@Model\.Collection\.(\w+)\((.+)\)";

        private static string ViewsFolderPath => $"{MvcContext.Get.RootDirectoryRelativePath}/{MvcContext.Get.ViewsFolderName}/";

        private static string ViewsSharedFolderPath => $"{ViewsFolderPath}/{MvcContext.Get.ViewsSharedFolderName}/";

        private static string ViewsDisplayTemplateFolderPath => $"{ViewsSharedFolderPath}/{DisplayTemplateFolderName}/";

        private static string FormatLayoutViewPath => $"{ViewsSharedFolderPath}/{MvcContext.Get.LayoutViewName}.{ViewExtension}";

        private static string FormatErrorViewPath => $"{ViewsSharedFolderPath}/{ErrorViewName}.{ViewExtension}";

        private static string FormatViewPath(string controllerName, string actionName)
            => $"{ViewsFolderPath}/{controllerName}/{actionName}.{ViewExtension}";

        private static string FormatDisplayTemplatePath(string objectName)
            => $"{ViewsDisplayTemplateFolderPath}/{objectName}{DisplayTemplateSuffix}.{ViewExtension}";

        private string ReadViewHtml(string viewPath)
        {
            if (!File.Exists(viewPath))
            {
                throw new FileNotFoundException("Template view could not be found.");
            }

            return File.ReadAllText(viewPath);
        }

        public string GetErrorContent() => ReadViewHtml(FormatLayoutViewPath).Replace("@RenderError()", ReadViewHtml(FormatErrorViewPath));

        public string GetViewContent(string controllerName, string actionName)
            => ReadViewHtml(FormatLayoutViewPath).Replace("@RenderBody()", ReadViewHtml(FormatViewPath(controllerName, actionName)));


        public string RenderHtml(string fullHtmlContent, IDictionary<string, object> viewData)
        {
            string renderedHtml = fullHtmlContent;

            if (viewData.Count > 0)
            {
                foreach (KeyValuePair<string, object> parameter in viewData)
                {
                    renderedHtml = RenderViewData(renderedHtml, parameter.Value, parameter.Key);
                }
            }

            if (viewData.ContainsKey("Error"))
            {
                renderedHtml = renderedHtml.Replace("@Error", viewData["Error"].ToString());
            }

            return renderedHtml;
        }

        private string RenderViewData(string template, object viewObject, string viewObjectName = null)
        {
            if (viewObject != null &&
                viewObject.GetType() != typeof(string) &&
                viewObject is IEnumerable collectionProperty &&
                Regex.IsMatch(template, ModelCollectionViewParameterPattern))
            {
                Match collectionMatch = Regex.Matches(template, ModelCollectionViewParameterPattern).First(cm => cm.Groups[1].Value == viewObjectName);

                string fullMatch = collectionMatch.Groups[0].Value;
                string itemPattern = collectionMatch.Groups[2].Value;

                StringBuilder result = new StringBuilder();

                foreach (object element in collectionProperty)
                {
                    result.Append(itemPattern).Replace("@Item", RenderViewData(template, element));
                }

                return template.Replace(fullMatch, result.ToString());
            }

            if (viewObject != null &&
                !viewObject.GetType().IsPrimitive &&
                viewObject.GetType() != typeof(string))
            {
                string objectDisplayTemplate = FormatDisplayTemplatePath(viewObject.GetType().Name);

                if (File.Exists(objectDisplayTemplate))
                {
                    string renderedObject = RenderObject(viewObject, File.ReadAllText(objectDisplayTemplate));

                    return viewObjectName != null ? template.Replace($"@Model.{viewObjectName}", renderedObject) : renderedObject;
                }
            }

            return viewObjectName != null ? template.Replace($"@Model.{viewObjectName}", viewObject?.ToString()) : viewObject?.ToString();
        }

        private string RenderObject(object viewObject, string displayTemplate)
        {
            Type viewObjectType = viewObject.GetType();
            PropertyInfo[] viewObjectProperties = viewObjectType.GetProperties();

            foreach (PropertyInfo viewObjectProperty in viewObjectProperties)
            {
                displayTemplate = RenderViewData(displayTemplate, viewObjectProperty.GetValue(viewObject), viewObjectProperty.Name);
            }

            return displayTemplate;
        }
    }
}
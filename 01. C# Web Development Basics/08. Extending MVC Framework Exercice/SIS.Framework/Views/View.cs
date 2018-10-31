namespace SIS.Framework.Views
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using SIS.Framework.ActionResults.Interfaces;
    using SIS.HTTP.Common;

    public class View : IRenderable
    {
        private readonly string _fullPathTemplateName;

        private readonly IDictionary<string, object> _viewData;

        public View(string fullPathTemplateName, IDictionary<string, object> viewData)
        {
            _fullPathTemplateName = fullPathTemplateName;
            _viewData = viewData;
        }

        private string ReadFile()
        {
            if (!File.Exists(_fullPathTemplateName))
            {
                throw new FileNotFoundException($"View does not exist at {_fullPathTemplateName}");
            }

            return File.ReadAllText(_fullPathTemplateName);
        }

        public string Render()
        {
            string fullHtml = ReadFile();
            string renderedHtml = RenderHtml(fullHtml);
            string layoutWithView = AddViewToLayout(renderedHtml);

            return layoutWithView;
        }

        private string AddViewToLayout(string renderedHtml)
        {
            string layoutViewPath = $"{MvcContext.Get.RootDirectoryRelativePath}{MvcContext.Get.ViewsFolderName}/{MvcContext.Get.LayoutViewName}{GlobalConstants.HtmlFileExtension}";

            if (!File.Exists(layoutViewPath))
            {
                throw new FileNotFoundException($"View does not exist at {layoutViewPath}");
            }

            string layoutViewContent = File.ReadAllText(layoutViewPath);
            string layoutView = layoutViewContent.Replace("@RenderBody", renderedHtml);

            return layoutView;
        }

        private string RenderHtml(string fullHtml)
        {
            if (_viewData.Any())
            {
                foreach (string viewDataKey in _viewData.Keys)
                {
                    string dataPlaceholder = $"{{{{{viewDataKey}}}}}";

                    if (fullHtml.Contains(dataPlaceholder))
                    {
                        fullHtml = fullHtml.Replace(dataPlaceholder, _viewData[viewDataKey].ToString());
                    }
                }
            }

            return fullHtml;
        }
    }
}
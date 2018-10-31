namespace SIS.Framework.Views
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using SIS.Framework.ActionResults.Interfaces;
    using SIS.HTTP.Common;

    public class View : IRenderable
    {
        private readonly string _fullHtmlContent;

        public View(string fullHtmlContent)
        {
            _fullHtmlContent = fullHtmlContent;
        }

        public string Render() => _fullHtmlContent;
    }
}
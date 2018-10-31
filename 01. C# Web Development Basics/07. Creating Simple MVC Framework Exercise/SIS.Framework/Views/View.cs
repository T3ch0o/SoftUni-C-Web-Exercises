namespace SIS.Framework.Views
{
    using System.IO;

    using SIS.Framework.ActionResults.Interfaces;

    public class View : IRenderable
    {
        private readonly string _fullPathTemplateName;

        public View(string fullPathTemplateName)
        {
            _fullPathTemplateName = fullPathTemplateName;
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

            return fullHtml;
        }
    }
}
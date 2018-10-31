namespace SIS.Framework.ActionResults
{
    using SIS.Framework.ActionResults.Interfaces;

    internal class ViewResult : IViewable
    {
        public ViewResult(IRenderable view)
        {
            View = view;
        }

        public IRenderable View { get; set; }

        public string Invoke() => View.Render();
    }
}
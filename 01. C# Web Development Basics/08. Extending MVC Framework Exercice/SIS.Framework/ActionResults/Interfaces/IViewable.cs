namespace SIS.Framework.ActionResults.Interfaces
{
    using SIS.Framework.ActionResults.Base;

    public interface IViewable : IActionResult
    {
        IRenderable View { get; set; }
    }
}
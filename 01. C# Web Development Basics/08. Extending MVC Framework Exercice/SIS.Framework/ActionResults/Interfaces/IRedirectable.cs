namespace SIS.Framework.ActionResults.Interfaces
{
    using SIS.Framework.ActionResults.Base;

    public interface IRedirectable : IActionResult
    {
        string RedirectUrl { get; }
    }
}
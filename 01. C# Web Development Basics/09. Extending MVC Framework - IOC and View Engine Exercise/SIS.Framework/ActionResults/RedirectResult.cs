namespace SIS.Framework.ActionResults
{
    using SIS.Framework.ActionResults.Interfaces;

    internal class RedirectResult : IRedirectable
    {
        public RedirectResult(string redirectUrl)
        {
            RedirectUrl = redirectUrl;
        }

        public string RedirectUrl { get; }

        public string Invoke() => RedirectUrl;
    }
}
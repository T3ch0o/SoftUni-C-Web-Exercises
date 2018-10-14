namespace SIS.WebServer.Result
{
    using System.Text;

    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;

    public class HtmlResult : HttpResponse
    {
        public HtmlResult(string content, HttpResponseStatusCode responseStatusCode) : base(responseStatusCode)
        {
            Headers.Add(new HttpHeader("Content-Type", "text/html"));
            Headers.Add(new HttpHeader("X-Frame-Options", "allow-from https://youtube.com/"));
            Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
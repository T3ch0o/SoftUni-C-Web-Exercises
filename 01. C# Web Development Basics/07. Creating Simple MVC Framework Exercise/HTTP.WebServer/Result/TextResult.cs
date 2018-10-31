namespace SIS.WebServer.Result
{
    using System.Text;

    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;

    internal class TextResult : HttpResponse
    {
        public TextResult(string content, HttpResponseStatusCode responseStatusCode) : base(responseStatusCode)
        {
            Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/plain"));
            Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
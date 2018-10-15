namespace SIS.HTTP.Responses
{
    using System.Linq;
    using System.Text;

    using SIS.HTTP.Common;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Extensions;
    using SIS.HTTP.Headers;

    public class HttpResponse : IHttpResponse
    {
        public HttpResponse()
        {
        }

        public HttpResponse(HttpResponseStatusCode statusCode)
        {
            CoreValidator.ThrowIfNull(statusCode, nameof(statusCode));

            Headers = new HttpHeaderCollection();
            Cookies = new HttpCookieCollection();
            Content = new byte[0];
            StatusCode = statusCode;
        }

        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }

        public HttpResponseStatusCode StatusCode { get; set; }

        public byte[] Content { get; set; }

        public void AddHeader(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, nameof(header));
            Headers.Add(header);
        }

        public void AddCookie(HttpCookie cookie)
        {
            CoreValidator.ThrowIfNull(cookie, nameof(cookie));
            Cookies.Add(cookie);
        }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(ToString()).Concat(Content).ToArray();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append($"{GlobalConstants.HttpOneProtocolFragment} {StatusCode.GetResponseLine()}").Append(GlobalConstants.HttpNewLine).Append(Headers).Append(GlobalConstants.HttpNewLine);

            if (Cookies.HasCookies())
                foreach (HttpCookie httpCookie in Cookies)
                    result.Append($"Set-Cookie: {httpCookie}").Append(GlobalConstants.HttpNewLine);

            result.Append(GlobalConstants.HttpNewLine);

            return result.ToString();
        }
    }
}
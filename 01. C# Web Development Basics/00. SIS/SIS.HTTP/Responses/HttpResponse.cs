namespace SIS.HTTP.Responses
{
    using System.Linq;
    using System.Text;

    using SIS.HTTP.Common;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Cookies.Interfaces;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Extensions;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Headers.Interfaces;
    using SIS.HTTP.Responses.Interfaces;

    public class HttpResponse : IHttpResponse
    {
        public HttpResponse()
        {
        }

        public HttpResponse(HttpResponseStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpResponseStatusCode StatusCode { get; }

        public IHttpHeaderCollection Headers { get; } = new HttpHeaderCollection();

        public IHttpCookieCollection Cookies { get; } = new HttpCookieCollection();

        public byte[] Content { get; set; } = new byte[0];

        public void AddHeader(HttpHeader header)
        {
            Headers.Add(header);
        }

        public void AddCookie(HttpCookie cookie)
        {
            Cookies.Add(cookie);
        }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(ToString()).Concat(Content).ToArray();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine($"{GlobalConstants.HttpOneProtocolFragment} {StatusCode.GetResponseLine()}")
                   .AppendLine(Headers.ToString());

            if (Cookies.HasCookies())
            {
                foreach (HttpCookie cookie in Cookies)
                {
                    result.AppendLine($"{GlobalConstants.HttpResponseCookieHeaderName}: {cookie}");
                }
            }

            result.AppendLine();

            return result.ToString();
        }
    }
}
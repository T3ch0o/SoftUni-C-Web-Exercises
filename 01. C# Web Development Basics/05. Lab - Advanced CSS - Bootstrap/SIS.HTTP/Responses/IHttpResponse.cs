namespace SIS.HTTP.Responses
{
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;

    public interface IHttpResponse
    {
        IHttpHeaderCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }

        HttpResponseStatusCode StatusCode { get; set; }

        byte[] Content { get; set; }

        void AddHeader(HttpHeader header);

        void AddCookie(HttpCookie cookie);

        byte[] GetBytes();
    }
}
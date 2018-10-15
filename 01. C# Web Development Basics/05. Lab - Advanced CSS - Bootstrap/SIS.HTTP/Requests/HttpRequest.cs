namespace SIS.HTTP.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SIS.HTTP.Common;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Exceptions;
    using SIS.HTTP.Extensions;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Sessions;

    public class HttpRequest : IHttpRequest
    {
        private const char HttpRequestUrlQuerySeparator = '?';

        private const char HttpRequestUrlFragmentSeparator = '#';

        private const string HttpRequestHeaderNameValueSeparator = ": ";

        private const string HttpRequestCookiesSeparator = "; ";

        private const char HttpRequestCookieNameValueSeparator = '=';

        private const char HttpRequestParameterSeparator = '&';

        private const char HttpRequestParameterNameValueSeparator = '=';

        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            FormData = new Dictionary<string, object>();
            QueryData = new Dictionary<string, object>();
            Headers = new HttpHeaderCollection();
            Cookies = new HttpCookieCollection();

            ParseRequest(requestString);
        }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpSession Session { get; set; }

        private bool IsValidRequestLine(string[] requestLine)
        {
            return requestLine.Length == 3 && requestLine[2].ToLower() != GlobalConstants.HttpOneProtocolFragment;
        }

        private bool IsValidRequestQueryString(string queryString, string[] queryParameters)
        {
            return !(string.IsNullOrEmpty(queryString) || queryParameters.Length < 1);
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            bool parseMethodResult = Enum.TryParse(requestLine[0].Capitalize(), out HttpRequestMethod parsedMethod);

            if (!parseMethodResult) throw new BadRequestException();

            RequestMethod = parsedMethod;
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            Url = requestLine[1];
        }

        private void ParseRequestPath()
        {
            Path = Url.Split(new[] { HttpRequestUrlQuerySeparator, HttpRequestUrlFragmentSeparator }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        private void ParseHeaders(string[] requestContent)
        {
            int currentIndex = 0;

            while (!string.IsNullOrEmpty(requestContent[currentIndex]))
            {
                string[] headerArguments = requestContent[currentIndex++].Split(HttpRequestHeaderNameValueSeparator);

                Headers.Add(new HttpHeader(headerArguments[0], headerArguments[1]));
            }

            if (!Headers.ContainsHeader(GlobalConstants.HostHeaderKey)) throw new BadRequestException();
        }

        private void ParseCookies()
        {
            if (!Headers.ContainsHeader(HttpHeader.Cookie)) return;

            string cookiesString = Headers.GetHeader(HttpHeader.Cookie).Value;

            if (string.IsNullOrEmpty(cookiesString)) return;

            string[] splitCookies = cookiesString.Split(HttpRequestCookiesSeparator);

            foreach (string splitCookie in splitCookies)
            {
                string[] cookieParts = splitCookie.Split(HttpRequestCookieNameValueSeparator, 2, StringSplitOptions.RemoveEmptyEntries);

                if (cookieParts.Length != 2) continue;

                string key = cookieParts[0];
                string value = cookieParts[1];

                Cookies.Add(new HttpCookie(key, value, false));
            }
        }

        private void ParseQueryParameters()
        {
            if (!Url.Contains('?')) return;

            string queryString = Url.Split(new[] { '?', '#' }, StringSplitOptions.None)[1];

            if (string.IsNullOrWhiteSpace(queryString)) return;

            string[] queryParameters = queryString.Split('&');

            if (!IsValidRequestQueryString(queryString, queryParameters)) throw new BadRequestException();

            foreach (string queryParameter in queryParameters)
            {
                string[] parameterArguments = queryParameter.Split('=', StringSplitOptions.RemoveEmptyEntries);

                QueryData.Add(parameterArguments[0], parameterArguments[1]);
            }
        }

        private void ParseFormDataParameters(string formData)
        {
            if (string.IsNullOrEmpty(formData)) return;

            string[] formDataParams = formData.Split(HttpRequestParameterSeparator);

            foreach (string formDataParameter in formDataParams)
            {
                string[] parameterArguments = formDataParameter.Split(HttpRequestParameterNameValueSeparator, StringSplitOptions.RemoveEmptyEntries);

                FormData.Add(parameterArguments[0], parameterArguments[1]);
            }
        }

        private void ParseRequestParameters(string formData)
        {
            ParseQueryParameters();
            ParseFormDataParameters(formData);
        }

        private void ParseRequest(string requestString)
        {
            string[] splitRequestContent = requestString.Split(new[] { GlobalConstants.HttpNewLine }, StringSplitOptions.None);

            string[] requestLine = splitRequestContent[0].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!IsValidRequestLine(requestLine)) throw new BadRequestException();

            ParseRequestMethod(requestLine);
            ParseRequestUrl(requestLine);
            ParseRequestPath();

            ParseHeaders(splitRequestContent.Skip(1).ToArray());
            ParseCookies();

            ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1]);
        }
    }
}
namespace SIS.HTTP.Cookies
{
    using System.Collections;
    using System.Collections.Generic;

    using SIS.HTTP.Common;

    public class HttpCookieCollection : IHttpCookieCollection
    {
        private const string HttpCookieStringSeparator = "; ";

        private readonly Dictionary<string, HttpCookie> cookies;

        public HttpCookieCollection()
        {
            cookies = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie cookie)
        {
            CoreValidator.ThrowIfNull(cookie, nameof(cookie));
            if (!ContainsCookie(cookie.Key)) cookies.Add(cookie.Key, cookie);
        }

        public bool ContainsCookie(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));
            return cookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));
            return cookies.GetValueOrDefault(key, null);
        }

        public bool HasCookies()
        {
            return cookies.Count > 0;
        }

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            foreach (KeyValuePair<string, HttpCookie> cookie in cookies) yield return cookie.Value;
        }

        public override string ToString()
        {
            return string.Join(HttpCookieStringSeparator, cookies.Values);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
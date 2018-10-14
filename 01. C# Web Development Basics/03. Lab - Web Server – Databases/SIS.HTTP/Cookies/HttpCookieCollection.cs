namespace SIS.HTTP.Cookies
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using SIS.HTTP.Cookies.Interfaces;

    internal class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly IDictionary<string, HttpCookie> cookies = new Dictionary<string, HttpCookie>();

        public void Add(HttpCookie cookie)
        {
            if (cookie == null)
            {
                throw new ArgumentNullException();
            }

            if (!ContainsCookie(cookie.Key))
            {
                cookies.Add(cookie.Key, cookie);
            }
        }

        public bool ContainsCookie(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException();
            }

            return cookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            if (!ContainsCookie(key))
            {
                return null;
            }

            return cookies[key];
        }

        public bool HasCookies() => cookies.Any();

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            foreach (KeyValuePair<string, HttpCookie> cookie in cookies)
            {
                yield return cookie.Value;
            }
        }

        public override string ToString()
        {
            return string.Join("; ", cookies.Values);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
namespace SIS.HTTP.Cookies
{
    using System;
    using System.Text;

    using SIS.HTTP.Common;

    public class HttpCookie
    {
        private const int HttpCookieDefaultExpirationDays = 3;
        private const string HttpCookieDefaultPath = "/";

        public HttpCookie(string key, string value, int expires = HttpCookieDefaultExpirationDays, string path = HttpCookieDefaultPath)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            Key = key;
            Value = value;
            IsNew = true;
            Path = path;
            Expires = DateTime.UtcNow.AddDays(expires);
        }

        public HttpCookie(string key, string value, bool isNew, int expires = HttpCookieDefaultExpirationDays, string path = HttpCookieDefaultPath) : this(key, value, expires)
        {
            IsNew = isNew;
        }

        public string Key { get; }

        public string Value { get; }

        public bool IsNew { get; }

        public DateTime Expires { get; private set; }

        public string Path { get; set; }

        public bool HttpOnly { get; set; } = true;

        public void Delete()
        {
            Expires = DateTime.UtcNow.AddDays(-1);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Key}={Value}; Expires={Expires:R}");

            if (HttpOnly) sb.Append("; HttpOnly");

            sb.Append($"; Path={Path}");

            return sb.ToString();
        }
    }
}
namespace SIS.HTTP.Cookies
{
    using System;

    public class HttpCookie
    {
        private const int DefaultExpirationDays = 3;

        public HttpCookie(string key, string value, int expires = DefaultExpirationDays)
        {
            Key = key;
            Value = value;
            Expires = DateTime.UtcNow.AddDays(expires);
            IsNew = true;
        }

        public HttpCookie(string key, string value, bool isNew, int expires = DefaultExpirationDays) : this(key, value, expires)
        {
            IsNew = isNew;
        }

        public string Key { get; }

        public string Value { get; private set; }

        public DateTime Expires { get; private set; }

        public bool IsNew { get; }

        public void Delete()
        {
            Expires = DateTime.UtcNow.AddDays(-1);
            Value = string.Empty;
        }

        public override string ToString()
        {
            return $"{Key}={Value}; Expires={Expires:R}; HttpOnly; Path=/";
        }
    }
}
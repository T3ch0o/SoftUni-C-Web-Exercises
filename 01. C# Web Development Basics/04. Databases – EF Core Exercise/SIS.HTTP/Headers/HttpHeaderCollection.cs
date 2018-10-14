namespace SIS.HTTP.Headers
{
    using System;
    using System.Collections.Generic;

    using SIS.HTTP.Headers.Interfaces;

    internal class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly IDictionary<string, HttpHeader> headers = new Dictionary<string, HttpHeader>();

        public void Add(HttpHeader header)
        {
            if (header == null || string.IsNullOrEmpty(header.Key) ||
                string.IsNullOrEmpty(header.Value) || ContainsHeader(header.Key))
            {
                throw new Exception();
            }

            headers.Add(header.Key, header);
        }

        public bool ContainsHeader(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException($"{nameof(key)} cannot be null");
            }

            return headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException($"{nameof(key)} cannot be null");
            }

            return headers.ContainsKey(key) ? headers[key] : throw new ArgumentException();
        }

        public override string ToString()
        {
            return string.Join("\n", headers.Values);
        }
    }
}
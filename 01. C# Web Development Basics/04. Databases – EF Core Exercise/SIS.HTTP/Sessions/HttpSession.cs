namespace SIS.HTTP.Session
{
    using System;
    using System.Collections.Generic;

    using SIS.HTTP.Session.Interfaces;

    internal class HttpSession : IHttpSession
    {
        private readonly IDictionary<string, object> parameters = new Dictionary<string, object>();

        public HttpSession(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public object GetParameter(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException();
            }

            if (!ContainsParameter(key))
            {
                return null;
            }

            return parameters[key];
        }

        public bool ContainsParameter(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException();
            }

            return parameters.ContainsKey(key);
        }

        public void AddParameter(string name, object parameter)
        {
            if (ContainsParameter(name))
            {
                throw new ArgumentException();
            }

            parameters.Add(name, parameter);
        }

        public void ClearParameters()
        {
            parameters.Clear();
        }
    }
}
namespace SIS.Framework.Models
{
    using System.Collections.Generic;

    public class ViewModel
    {
        public IDictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        public object this[string key]
        {
            get => Data[key];
            set => Data[key] = value;
        }
    }
}
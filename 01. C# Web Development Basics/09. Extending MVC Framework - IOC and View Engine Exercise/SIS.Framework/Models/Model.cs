namespace SIS.Framework.Models
{
    using System.Collections.Generic;

    public class Model
    {
        private bool? _isValid;

        public IDictionary<string, string> modelErrors;

        public bool? IsValid
        {
            get => _isValid;
            set => _isValid = _isValid ?? value;
        }
    }
}
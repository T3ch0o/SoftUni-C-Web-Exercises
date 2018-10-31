namespace SIS.Framework.Attributes.Properties
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class RegexAttribute : ValidationAttribute
    {
        private readonly string _pattern;

        public RegexAttribute(string pattern)
        {
            _pattern = pattern;
        }

        public override bool IsValid(object value)
        {
            return Regex.IsMatch(value.ToString(), _pattern);
        }
    }
}
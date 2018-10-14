namespace SIS.HTTP.Extensions
{
    using System;

    internal static class StringExtensions
    {
        public static string Capitalize(this string message)
        {
            return string.IsNullOrEmpty(message) ? 
                    throw new ArgumentException($"{nameof(message)} cannot be null") : 
                    char.ToUpper(message[0]) + message.Substring(1).ToLower();
        }
    }
}
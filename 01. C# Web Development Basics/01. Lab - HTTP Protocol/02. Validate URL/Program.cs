namespace _02.Validate_URL
{
    using System;
    using System.Net;
    using System.Text;

    using static System.Console;

    internal static class Program
    {
        private static void Main()
        {
            try
            {
                Uri uriTest = new Uri(WebUtility.UrlDecode(ReadLine()));

                StringBuilder result = new StringBuilder();

                if (!uriTest.IsWellFormedOriginalString()) result.AppendLine("Invalid URL");

                bool validHttp = uriTest.Port == 80 && uriTest.Scheme == "http";
                bool validHttps = uriTest.Port == 443 && uriTest.Scheme == "https";

                if (validHttps || validHttp)
                {
                    result.AppendLine($"Protocol: {uriTest.Scheme}");
                    result.AppendLine($"Host: {uriTest.DnsSafeHost}");
                    result.AppendLine($"Port: {uriTest.Port}");
                    string path = WebUtility.UrlDecode(uriTest.LocalPath);
                    result.AppendLine($"Path: {path}");

                    string query = WebUtility.UrlDecode(uriTest.Query);
                    if (uriTest.Query != string.Empty) result.AppendLine($"Query: {query.TrimStart('?')}");

                    string fragment = WebUtility.UrlDecode(uriTest.Fragment);
                    if (uriTest.Fragment != string.Empty) result.AppendLine($"Fragment: {fragment.TrimStart('#')}");
                }
                else
                    result.AppendLine("Invalid URL");

                WriteLine(result.ToString().Trim());
            }
            catch (Exception ex)
            {
                WriteLine("Invalid URL");
            }
        }
    }
}
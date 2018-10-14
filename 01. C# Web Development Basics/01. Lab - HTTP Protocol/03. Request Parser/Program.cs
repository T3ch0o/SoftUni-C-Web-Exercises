namespace _03.Request_Parser
{
    using System;
    using System.Collections.Generic;

    using static System.Console;

    internal static class Program
    {
        private static void Main()
        {
            Dictionary<string, HashSet<string>> validUrls = new Dictionary<string, HashSet<string>>();

            string input;
            while ((input = ReadLine()) != "END")
            {
                string[] urlParts = input.Split('/');
                string path = $"/{urlParts[0]}";
                string method = urlParts[1];

                if (!validUrls.ContainsKey(path)) validUrls[path] = new HashSet<string>();

                validUrls[path].Add(method);
            }

            string request = ReadLine();
            string[] reqParts = request.Split();

            string reqMethod = reqParts[0];
            string reqUrl = reqParts[1];
            string reqProtocol = reqParts[2];

            int responseCode = 404;
            string responseStatusText = "Not Found";

            if (validUrls.ContainsKey(reqUrl) && validUrls[reqUrl].Contains(reqMethod.ToLower()))
            {
                responseCode = 200;
                responseStatusText = "OK";
            }

            WriteLine($"{reqProtocol} {responseCode} {responseStatusText}");
            WriteLine($"Content-Length: {responseStatusText.Length}");
            WriteLine("Content-Type-text/plain\r\n");
            WriteLine(responseStatusText);
        }
    }
}
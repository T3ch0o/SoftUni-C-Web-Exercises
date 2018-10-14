namespace _01.URL_Decode
{
    using System.Net;

    using static System.Console;

    internal static class Program
    {
        private static void Main()
        {
            string decodedUrl = WebUtility.UrlDecode(ReadLine());

            WriteLine(decodedUrl);
        }
    }
}
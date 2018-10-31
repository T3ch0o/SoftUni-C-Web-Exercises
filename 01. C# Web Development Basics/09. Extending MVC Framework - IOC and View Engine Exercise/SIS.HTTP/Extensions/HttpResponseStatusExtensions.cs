namespace SIS.HTTP.Extensions
{
    using SIS.HTTP.Enums;

    internal static class HttpResponseStatusExtensions
    {
        public static string GetResponseLine(this HttpResponseStatusCode statusCode) => $"{(int)statusCode} {statusCode}";
    }
}
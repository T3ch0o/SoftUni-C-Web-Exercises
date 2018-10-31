namespace SIS.WebServer.Api.Interfaces
{
    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses.Interfaces;

    public interface IHttpHandlingContext
    {
        IHttpResponse Handle(IHttpRequest request);
    }
}
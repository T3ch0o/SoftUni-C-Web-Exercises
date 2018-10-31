namespace SIS.WebServer.Api.Interfaces
{
    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses.Interfaces;

    public interface IHttpHandler
    {
        IHttpResponse Handle(IHttpRequest httpRequest);
    }
}
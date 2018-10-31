namespace SIS.Framework.Routers
{
    using System.Linq;

    using SIS.HTTP.Common;
    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses.Interfaces;
    using SIS.WebServer.Api.Interfaces;

    public class HttpRouteHandlingContext : IHttpHandlingContext
    {
        public HttpRouteHandlingContext(IHttpHandler controllerHandler, IHttpHandler resourceHandler)
        {
            ControllerHandler = controllerHandler;
            ResourceHandler = resourceHandler;
        }

        private IHttpHandler ControllerHandler { get; }

        private IHttpHandler ResourceHandler { get; }

        public IHttpResponse Handle(IHttpRequest request)
        {
            if (IsResourceRequest(request))
            {
                return ResourceHandler.Handle(request);
            }

            return ControllerHandler.Handle(request);
        }

        private bool IsResourceRequest(IHttpRequest httpRequest)
        {
            string requestPath = httpRequest.Path;

            if (requestPath.Contains('.'))
            {
                string extension = requestPath.Substring(requestPath.LastIndexOf('.'));
                return GlobalConstants.ResourceExtension.Contains(extension);
            }

            return false;
        }
    }
}
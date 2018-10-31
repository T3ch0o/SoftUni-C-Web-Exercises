namespace SIS.WebServer.Api
{
    using System.IO;
    using System.Linq;

    using global::HTTP.WebServer.Result;

    using SIS.HTTP.Common;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Responses.Interfaces;
    using SIS.WebServer.Api.Interfaces;
    using SIS.WebServer.Routing;

    public class HttpHandler : IHttpHandler
    {
        private const string RootDirectoryRelativePath = "../../..";

        private ServerRoutingTable _serverRoutingTable;

        public HttpHandler(ServerRoutingTable serverRoutingTable)
        {
            _serverRoutingTable = serverRoutingTable;
        }

        public IHttpResponse Handle(IHttpRequest httpRequest)
        {
            bool isResourceRequest = IsResourceRequest(httpRequest);

            if (isResourceRequest)
            {
                return HandlerRequestResponse(httpRequest.Path);
            }

            if (!_serverRoutingTable.Routes.ContainsKey(httpRequest.RequestMethod) || !_serverRoutingTable.Routes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path.ToLower()))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            return _serverRoutingTable.Routes[httpRequest.RequestMethod][httpRequest.Path].Invoke(httpRequest);
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

        private IHttpResponse HandlerRequestResponse(string httpRequestPath)
        {
            int indexOfExtension = httpRequestPath.LastIndexOf('.');
            int indexOfNameOfResource = httpRequestPath.LastIndexOf('/');

            string extension = httpRequestPath.Substring(indexOfExtension);

            string resourceName = httpRequestPath.Substring(indexOfNameOfResource);

            string resourcePath = $"{RootDirectoryRelativePath}/Resources/{extension.Substring(1)}{resourceName}";

            if (!File.Exists(resourcePath))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            byte[] fileContent = File.ReadAllBytes(resourcePath);

            return new InlineResourceResult(fileContent, HttpResponseStatusCode.OK);
        }
    }
}
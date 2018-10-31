namespace SIS.Framework.Routers
{
    using System.IO;

    using global::HTTP.WebServer.Result;

    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Responses.Interfaces;
    using SIS.WebServer.Api.Interfaces;

    public class ResourceRouter : IHttpHandler
    {
        public IHttpResponse Handle(IHttpRequest httpRequest)
        {
            string httpRequestPath = httpRequest.Path;

            int indexOfExtension = httpRequestPath.LastIndexOf('.');
            int indexOfNameOfResource = httpRequestPath.LastIndexOf('/');

            string extension = httpRequestPath.Substring(indexOfExtension);

            string resourceName = httpRequestPath.Substring(indexOfNameOfResource);

            string resourcePath = $"{MvcContext.Get.RootDirectoryRelativePath}/{MvcContext.Get.ResourceFolderName}/{extension.Substring(1)}{resourceName}";

            if (!File.Exists(resourcePath))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            byte[] fileContent = File.ReadAllBytes(resourcePath);

            return new InlineResourceResult(fileContent, HttpResponseStatusCode.OK);
        }
    }
}
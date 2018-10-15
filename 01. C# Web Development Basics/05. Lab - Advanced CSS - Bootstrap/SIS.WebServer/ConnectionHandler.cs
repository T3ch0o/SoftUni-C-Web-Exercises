namespace SIS.WebServer
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    using SIS.HTTP.Common;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Exceptions;
    using SIS.HTTP.Requests;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Sessions;
    using SIS.WebServer.Results;
    using SIS.WebServer.Routing;

    public class ConnectionHandler
    {
        private const string RootDirectoryRelativePath = "../../..";
        private readonly Socket client;

        private readonly ServerRoutingTable serverRoutingTable;

        public ConnectionHandler(Socket client, ServerRoutingTable serverRoutingTable)
        {
            CoreValidator.ThrowIfNull(client, nameof(client));
            CoreValidator.ThrowIfNull(serverRoutingTable, nameof(serverRoutingTable));

            this.client = client;
            this.serverRoutingTable = serverRoutingTable;
        }

        public async Task ProcessRequestAsync()
        {
            try
            {
                IHttpRequest httpRequest = await ReadRequest();

                if (httpRequest != null)
                {
                    string sessionId = SetRequestSession(httpRequest);

                    IHttpResponse httpResponse = HandleRequest(httpRequest);

                    SetResponseSession(httpResponse, sessionId);

                    await PrepareResponse(httpResponse);
                }
            }
            catch (BadRequestException e)
            {
                await PrepareResponse(new TextResult(e.Message, HttpResponseStatusCode.BadRequest));
            }
            catch (Exception e)
            {
                await PrepareResponse(new TextResult(e.Message, HttpResponseStatusCode.InternalServerError));
            }

            client.Shutdown(SocketShutdown.Both);
        }

        private async Task<IHttpRequest> ReadRequest()
        {
            StringBuilder result = new StringBuilder();
            ArraySegment<byte> data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int numberOfBytesRead = await client.ReceiveAsync(data.Array, SocketFlags.None);

                if (numberOfBytesRead == 0) break;

                string bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);
                result.Append(bytesAsString);

                if (numberOfBytesRead < 1023) break;
            }

            if (result.Length == 0) return null;

            return new HttpRequest(result.ToString());
        }

        private IHttpResponse HandleRequest(IHttpRequest httpRequest)
        {
            bool isResourceRequest = IsResourceRequest(httpRequest);
            if (isResourceRequest) return HandleRequestResponse(httpRequest.Path);
            if (!serverRoutingTable.Routes.ContainsKey(httpRequest.RequestMethod) || !serverRoutingTable.Routes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path.ToLower()))
                return new HttpResponse(HttpResponseStatusCode.NotFound);

            return serverRoutingTable.Routes[httpRequest.RequestMethod][httpRequest.Path].Invoke(httpRequest);
        }

        private IHttpResponse HandleRequestResponse(string httpRequestPath)
        {
            int indexOfStartOfExtension = httpRequestPath.LastIndexOf('.');
            int indexOfStartOfNameOfResource = httpRequestPath.LastIndexOf('/');

            string requestPathExtension = httpRequestPath.Substring(indexOfStartOfExtension);

            string resourceName = httpRequestPath.Substring(indexOfStartOfNameOfResource);

            string resourcePath = RootDirectoryRelativePath + "/Resources" + $"/{requestPathExtension.Substring(1)}" + resourceName;

            if (!File.Exists(resourcePath)) return new HttpResponse(HttpResponseStatusCode.NotFound);

            byte[] fileContent = File.ReadAllBytes(resourcePath);

            return new InlineResourceResult(fileContent, HttpResponseStatusCode.Ok);
        }

        private bool IsResourceRequest(IHttpRequest httpRequest)
        {
            string requestPath = httpRequest.Path;
            if (requestPath.Contains('.'))
            {
                string requestPathExtension = requestPath.Substring(requestPath.LastIndexOf('.'));
                return GlobalConstants.ResourceExtensions.Contains(requestPathExtension);
            }

            return false;
        }

        private async Task PrepareResponse(IHttpResponse httpResponse)
        {
            byte[] byteSegments = httpResponse.GetBytes();

            await client.SendAsync(byteSegments, SocketFlags.None);
        }

        private string SetRequestSession(IHttpRequest httpRequest)
        {
            string sessionId = null;

            if (httpRequest.Cookies.ContainsCookie(HttpSessionStorage.SessionCookieKey))
            {
                HttpCookie cookie = httpRequest.Cookies.GetCookie(HttpSessionStorage.SessionCookieKey);
                sessionId = cookie.Value;
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }

            return sessionId;
        }

        private void SetResponseSession(IHttpResponse httpResponse, string sessionId)
        {
            if (sessionId != null)
                httpResponse.AddCookie(new HttpCookie(HttpSessionStorage.SessionCookieKey, sessionId));
        }
    }
}
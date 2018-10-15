using System.Net.Sockets;
using SIS.WebServer.Routing;

namespace SIS.WebServer
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using global::HTTP.WebServer.Result;

    using SIS.HTTP.Common;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests;
    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Responses.Interfaces;
    using SIS.HTTP.Sessions;

    internal class ConnectionHandler
    {
        private const string RootDirectoryRelativePath = "../../..";

        private readonly Socket _client;

        private readonly ServerRoutingTable _serverRoutingTable;

        public ConnectionHandler(Socket client, ServerRoutingTable serverRoutingTable)
        {
            _client = client;
            _serverRoutingTable = serverRoutingTable;
        }

        private async Task<IHttpRequest> ReadRequest()
        {
            StringBuilder result = new StringBuilder();
            ArraySegment<byte> data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int numberOfBytesRead = await _client.ReceiveAsync(data, SocketFlags.None);

                if (numberOfBytesRead == 0)
                {
                    break;
                }

                string bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);
                result.Append(bytesAsString);

                if (numberOfBytesRead < 1023)
                {
                    break;
                }
            }

            return result.Length == 0 ? null : new HttpRequest(result.ToString());
        }

        private IHttpResponse HandleRequest(IHttpRequest httpRequest)
        {
            bool isResourceRequest = IsResourceRequest(httpRequest);

            if (isResourceRequest)
            {
                return HandlerRequestResponse(httpRequest.Path);
            }

            if (!_serverRoutingTable.Routes.ContainsKey(httpRequest.RequestMethod)
                || !_serverRoutingTable.Routes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path.ToLower()))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            return _serverRoutingTable.Routes[httpRequest.RequestMethod][httpRequest.Path].Invoke(httpRequest);
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

        private async Task PrepareResponse(IHttpResponse httpResponse)
        {
            ArraySegment<byte> byteSegments = new ArraySegment<byte>(httpResponse.GetBytes());

            await _client.SendAsync(byteSegments, SocketFlags.None);
        }

        public async Task ProcessRequestAsync()
        {
            IHttpRequest httpRequest = await ReadRequest();

            if (httpRequest != null)
            {
                string sessionId = SetRequestSession(httpRequest);

                IHttpResponse httpResponse = HandleRequest(httpRequest);

                SetResponseSession(httpResponse, sessionId);

                await PrepareResponse(httpResponse);
            }

            _client.Shutdown(SocketShutdown.Both);
        }

        private string SetRequestSession(IHttpRequest request)
        {
            string sessionId;

            if (request.Cookies.ContainsCookie(HttpSessionStorage.SessionCookieKey))
            {
                HttpCookie cookie = request.Cookies.GetCookie(HttpSessionStorage.SessionCookieKey);
                sessionId = cookie.Value;
                request.Session = HttpSessionStorage.GetSession(sessionId);
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
                request.Session = HttpSessionStorage.GetSession(sessionId);
            }

            return sessionId;
        }

        private void SetResponseSession(IHttpResponse response, string sessionId)
        {
            if (!string.IsNullOrEmpty(sessionId))
            {
                response.AddCookie(new HttpCookie(HttpSessionStorage.SessionCookieKey, sessionId));
            }
        }
    }
}
using System.Net.Sockets;
using SIS.WebServer.Routing;

namespace SIS.WebServer
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    using SIS.HTTP.Cookies;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests;
    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Responses.Interfaces;
    using SIS.HTTP.Sessions;

    internal class ConnectionHandler
    {
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
            if (!_serverRoutingTable.Routes.ContainsKey(httpRequest.RequestMethod)
                || !_serverRoutingTable.Routes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path.ToLower()))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            return _serverRoutingTable.Routes[httpRequest.RequestMethod][httpRequest.Path].Invoke(httpRequest);
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
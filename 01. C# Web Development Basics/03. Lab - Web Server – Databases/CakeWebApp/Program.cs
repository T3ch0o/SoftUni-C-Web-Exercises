namespace CakeWebApp
{
    using System;

    using SIS.HTTP.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Routing;

    using CakeWebApp.Controllers;

    using static System.Console;

    internal static class Program
    {
        private static void Main()
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/register"] = request => new AccountController().RegisterGet(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/register"] = request => new AccountController().RegisterPost(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/login"] = request => new AccountController().LoginGet(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/login"] = request => new AccountController().LoginPost(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/logout"] = request => new AccountController().Logout(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/hello"] = request => new HomeController().Asd(request);


            Server server = new Server(80, serverRoutingTable);

            server.Run();
        }
    }
}
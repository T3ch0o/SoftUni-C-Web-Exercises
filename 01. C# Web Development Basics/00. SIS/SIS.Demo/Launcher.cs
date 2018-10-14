namespace SIS.Demo
{
    using System;
    using System.Collections.Generic;

    using SIS.HTTP.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Routing;

    using static System.Console;

    internal static class Launcher
    {
        private static void Main()
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index();

            Server server = new Server(80, serverRoutingTable);

            server.Run();
        }
    }
}
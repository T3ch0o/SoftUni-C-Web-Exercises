namespace IRunes
{
    using HTTP.WebServer.Result;

    using IRunes.Controllers;

    using SIS.HTTP.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Result;
    using SIS.WebServer.Routing;

    internal class Program
    {
        private static void Main()
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            ConfiguringRouting(serverRoutingTable);

            Server server = new Server(80, serverRoutingTable);

            server.Run();
        }

        private static void ConfiguringRouting(ServerRoutingTable serverRoutingTable)
        {
            // Index Routes
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/home/index"] = request => new RedirectResult("/");
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index(request);

            // Authentication Routes
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/login"] = request => new UsersController().LoginGet(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/login"] = request => new UsersController().LoginPost(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/register"] = request => new UsersController().RegisterGet(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/register"] = request => new UsersController().RegisterPost(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/logout"] = request => new UsersController().Logout(request);

            // Albums Routes
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/all"] = request => new AlbumsController().All(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/create"] = request => new AlbumsController().CreateGet(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/albums/create"] = request => new AlbumsController().CreatePost(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/details"] = request => new AlbumsController().Details(request);
 
            // Tracks Routes
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/tracks/create"] = request => new TracksController().CreateGet(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/tracks/details"] = request => new TracksController().Details(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/tracks/create"] = request => new TracksController().CreatePost(request);
        }
    }
}

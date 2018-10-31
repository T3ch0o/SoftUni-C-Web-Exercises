namespace SIS.Demo
{
    using SIS.Framework;
    using SIS.Framework.Routers;
    using SIS.WebServer;

    class Launcher
    {
        private static void Main()
        {
            Server sever = new Server(80, new HttpRouteHandlingContext(
                new ControllerRouter(),
                new ResourceRouter()));

            MvcEngine.Run(sever);
        }
    }
}

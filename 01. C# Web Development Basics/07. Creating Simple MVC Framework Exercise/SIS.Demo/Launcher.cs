namespace SIS.Demo
{
    using SIS.Framework.Routers;
    using SIS.WebServer;
    using MvcEngine;

    class Launcher
    {
        private static void Main()
        {
            Server sever = new Server(80, new ControllerRouter());

            MvcEngine.Run(sever);
        }
    }
}

namespace SIS.Demo
{
    using System;
    using System.Collections.Generic;

    using SIS.Demo.Services;
    using SIS.Demo.Services.Interfaces;
    using SIS.Framework;
    using SIS.Framework.Routers;
    using SIS.Framework.Services;
    using SIS.WebServer;

    class Launcher
    {
        private static void Main()
        {
            Dictionary<Type, Type> dependencyMap = new Dictionary<Type, Type>();

            DependencyContainer dependencyContainer = new DependencyContainer(dependencyMap);
            dependencyContainer.RegisterDependency<IHashService, HashService>();

            Server sever = new Server(80, new HttpRouteHandlingContext(
                new ControllerRouter(dependencyContainer),
                new ResourceRouter()));

            MvcEngine.Run(sever);
        }
    }
}

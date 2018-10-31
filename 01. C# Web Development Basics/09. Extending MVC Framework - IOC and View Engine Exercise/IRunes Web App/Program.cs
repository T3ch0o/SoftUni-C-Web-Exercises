namespace IRunes
{
    using System;
    using System.Collections.Generic;

    using IRunes.Services;
    using IRunes.Services.Interfaces;

    using SIS.Framework;
    using SIS.Framework.Routers;
    using SIS.Framework.Services;
    using SIS.WebServer;

    internal class Program
    {
        private static void Main()
        {
            Dictionary<Type, Type> dependencyMap = new Dictionary<Type, Type>();

            DependencyContainer dependencyContainer = new DependencyContainer(dependencyMap);
            dependencyContainer.RegisterDependency<IHashService, HashService>();
            dependencyContainer.RegisterDependency<IUsersService, UsersService>();

            Server sever = new Server(80, new HttpRouteHandlingContext(new ControllerRouter(dependencyContainer), new ResourceRouter()));

            MvcEngine.Run(sever);
        }
    }
}

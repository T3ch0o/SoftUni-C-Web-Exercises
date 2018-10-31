namespace MvcEngine
{
    using System;
    using System.Reflection;

    using SIS.Framework;
    using SIS.WebServer;

    public static class MvcEngine
    {
        public static void Run(Server server)
        {
            RegisterAssemblyName();

            try
            {
                server.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void RegisterAssemblyName()
        {
            MvcContext.Get.AssemblyName = Assembly.GetEntryAssembly().GetName().Name;
        }
    }
}

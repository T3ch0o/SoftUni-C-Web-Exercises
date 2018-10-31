namespace SIS.Framework
{
    public class MvcContext
    {
        private static MvcContext Instance;

        private MvcContext() { }

        public static MvcContext Get => Instance ?? (Instance = new MvcContext());

        public string AssemblyName { get; set; }

        public string ControllerSuffix { get; set; } = "Controller";

        public string ControllersFolderName { get; set; } = "Controllers";

        public string ViewsFolderName { get; set; } = "Views";

        public string ViewsSharedFolderName { get; set; } = "Shared";

        public string ModelsFolderName { get; set; } = "Models";

        public string ResourceFolderName { get; set; } = "Resources";

        public string LayoutViewName { get; set; } = "_Layout";

        public string RootDirectoryRelativePath { get; set; } = "../../../";
    }
}
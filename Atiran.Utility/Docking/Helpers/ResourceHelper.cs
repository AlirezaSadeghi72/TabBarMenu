using System.Resources;

namespace Atiran.Utility.Docking
{
    internal static class ResourceHelper
    {
        private static ResourceManager _resourceManager;

        private static ResourceManager ResourceManager
        {
            get
            {
                if (_resourceManager == null)
                    _resourceManager =
                        new ResourceManager("Atiran.Utility.Docking.Strings", typeof(ResourceHelper).Assembly);
                return _resourceManager;
            }
        }

        public static string GetString(string name)
        {
            return ResourceManager.GetString(name);
        }
    }
}
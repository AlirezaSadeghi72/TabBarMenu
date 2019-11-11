using System.Configuration;

namespace Atiran.Utility.Docking2.Configuration
{
    public class PatchSection : ConfigurationSection
    {
        [ConfigurationProperty("enableAll", DefaultValue = null)]
        public bool? EnableAll => (bool?) base["enableAll"];

        [ConfigurationProperty("enableHighDpi", DefaultValue = true)]
        public bool EnableHighDpi => (bool) base["enableHighDpi"];

        [ConfigurationProperty("enableMemoryLeakFix", DefaultValue = true)]
        public bool EnableMemoryLeakFix => (bool) base["enableMemoryLeakFix"];

        [ConfigurationProperty("enableMainWindowFocusLostFix", DefaultValue = true)]
        public bool EnableMainWindowFocusLostFix => (bool) base["enableMainWindowFocusLostFix"];

        [ConfigurationProperty("enableNestedDisposalFix", DefaultValue = true)]
        public bool EnableNestedDisposalFix => (bool) base["enableNestedDisposalFix"];

        [ConfigurationProperty("enableFontInheritanceFix", DefaultValue = true)]
        public bool EnableFontInheritanceFix => (bool) base["enableFontInheritanceFix"];

        [ConfigurationProperty("enableContentOrderFix", DefaultValue = true)]
        public bool EnableContentOrderFix => (bool) base["enableContentOrderFix"];

        [ConfigurationProperty("enableActiveXFix", DefaultValue = false)] // disabled by default to avoid side effect.
        public bool EnableActiveXFix => (bool) base["enableActiveXFix"];

        [ConfigurationProperty("enableDisplayingPaneFix", DefaultValue = true)]
        public bool EnableDisplayingPaneFix => (bool) base["enableDisplayingPaneFix"];

        [ConfigurationProperty("enableActiveControlFix", DefaultValue = true)]
        public bool EnableActiveControlFix => (bool) base["enableActiveControlFix"];

        [ConfigurationProperty("enableFloatSplitterFix", DefaultValue = true)]
        public bool EnableFloatSplitterFix => (bool) base["enableFloatSplitterFix"];

        [ConfigurationProperty("enableActivateOnDockFix", DefaultValue = true)]
        public bool EnableActivateOnDockFix => (bool) base["enableActivateOnDockFix"];

        [ConfigurationProperty("enableSelectClosestOnClose", DefaultValue = true)]
        public bool EnableSelectClosestOnClose => (bool) base["enableSelectClosestOnClose"];
    }
}
using Atiran.Utility.Docking2.Theme.ThemeVS2013;

namespace Atiran.Utility.Docking2
{
    /// <summary>
    ///     Visual Studio 2013 Light theme.
    /// </summary>
    public class VS2013LightTheme : VS2013ThemeBase
    {
        public VS2013LightTheme()
            : base(Decompress(Resources.vs2013light_vstheme))
        {
        }
    }
}
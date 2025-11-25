using MudBlazor;

namespace StudentAllowanceTracker.Client.Components.CustomTheme
{
    public static class ThemeColor
    {
        public static readonly MudTheme Theme = new MudTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = "#0EA87E",
                PrimaryDarken = "#057A58",
                PrimaryLighten = "#D5F3E8",

                Success = "#0EA87E",
                Error = "#EF4444",
                Warning = "#F59E0B",
                Info = "#3B82F6",

                Background = "#FAFAFA",
                Surface = "#FFFFFF",
                DrawerBackground = "#F9FFF8",

                TextPrimary = "rgba(27, 27, 27, 0.87)",
                TextSecondary = "rgba(133, 133, 133, 0.6)",
                TextDisabled = "rgba(207, 207, 207, 0.38)",

                Divider = "#D5F3E8",
                DividerLight = "#CFCFCF",

                AppbarBackground = "#FFFFFF",
                AppbarText = "#1B1B1B",
                DrawerText = "#1B1B1B"
            },

            PaletteDark = new PaletteDark()
            {
                Primary = "#0EA87E",
                Background = "#1A1A1A",
                Surface = "#242424",
                TextPrimary = "#FFFFFF",
            },

            LayoutProperties = new LayoutProperties()
            {
                DefaultBorderRadius = "0.5rem"
            }
        };
    }
}

using MudBlazor;

namespace StudentAllowanceTracker.Client.Components.CustomTheme
{
    public class ThemeColor
    {
        public MudTheme customTheme = new MudTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = "#FFC107",
                Secondary = "#FF9800",
                AppbarBackground = "#000000",
                Background = "#FFFFFF",
                Surface = "#FFFFFF",
                TextPrimary = "#1a1a1a",
                TextSecondary = "#666666",
                Warning = "#FFC107"
            },
            PaletteDark = new PaletteDark()
            {
                Primary = "#FFC107",
                Secondary = "#FF9800",
                AppbarBackground = "#000000",
                Background = "#000000",
                Surface = "#1a1a1a",
                TextPrimary = "#FFFFFF",
                TextSecondary = "rgba(255,255,255,0.7)",
                Warning = "#FFC107"
            }
        };
    }
}

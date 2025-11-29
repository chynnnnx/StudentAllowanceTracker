using MudBlazor;

namespace StudentAllowanceTracker.Client.Components.Common
{
    public class PasswordField
    {
        public bool IsVisible { get; set; } = false;
        public InputType InputType => IsVisible ? InputType.Text : InputType.Password;
        public string Icon => IsVisible ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;

        public void Toggle() => IsVisible = !IsVisible;
    }
}

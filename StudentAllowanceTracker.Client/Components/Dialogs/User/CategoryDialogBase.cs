using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Client.Components.Dialogs.User
{
    public class CategoryDialogBase: LayoutComponentBase

    {
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;

        [Parameter]
        public CategoryDTO CategoryForm { get; set; } = new();

        [Parameter]
        public bool IsEditing { get; set; } = false;

        [Parameter]
        public EventCallback<CategoryDTO> OnSubmit { get; set; }

        protected void Submit()
        {
            if (IsFormValid())
            {
                MudDialog.Close(DialogResult.Ok(CategoryForm));
            }
        }

        protected void Cancel()
        {
            MudDialog.Cancel();
        }

        protected bool IsFormValid()
        {
            return !string.IsNullOrWhiteSpace(CategoryForm.CategoryName);
        }

        protected string GetTypeCardStyle(CategoryType type)
        {
            var isSelected = CategoryForm.Type == type;
            var (color, _, bgColor) = GetTypeStyle(type);

            if (isSelected)
            {
                return $"border-color: {color}; background-color: {bgColor}; border-radius: 0.75rem;";
            }
            return $"border-color: hsl(0, 0%, 85%); background-color: white; border-radius: 0.75rem;";
        }

        protected string GetTypeIconStyle(CategoryType type)
        {
            var isSelected = CategoryForm.Type == type;
            var (color, _, _) = GetTypeStyle(type);

            return $"color: {color}; font-size: 2rem; opacity: {(isSelected ? "1" : "0.4")};";
        }

        protected (string color, string icon, string bgColor) GetTypeStyle(CategoryType type)
        {
            return type switch
            {
                CategoryType.Needs => ("hsl(162, 86.6%, 32.2%)", Icons.Material.Filled.Home, "hsl(162, 41.8%, 95%)"),
                CategoryType.Wants => ("hsl(280, 86.6%, 60%)", Icons.Material.Filled.Favorite, "hsl(280, 41.8%, 95%)"),
                CategoryType.Savings => ("hsl(210, 86.6%, 50%)", Icons.Material.Filled.Savings, "hsl(210, 41.8%, 95%)"),
                _ => ("hsl(162, 86.6%, 32.2%)", Icons.Material.Filled.Category, "hsl(162, 41.8%, 95%)")
            };
        }

    }
}

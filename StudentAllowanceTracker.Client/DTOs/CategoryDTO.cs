using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Client.DTOs
{
    public class CategoryDTO
    {
        public Guid CategoryID { get; set; }
        public string UserID { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;
        public CategoryType Type { get; set; } = CategoryType.Needs;  // enum for 50/30/20
        public decimal? BudgetAmount { get; set; }

        public override string ToString()
        {
            return CategoryName;
        }

        public override bool Equals(object? obj)
        {
            return obj is CategoryDTO dto && CategoryID == dto.CategoryID;
        }

        public override int GetHashCode() => CategoryID.GetHashCode();
    }
}

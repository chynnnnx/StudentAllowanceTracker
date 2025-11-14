namespace StudentAllowanceTracker.Client.DTOs
{
    public class ExpenseDTO
    {
        public Guid ExpenseID { get; set; }
        public string UserID { get; set; } = string.Empty;
        public Guid AllowanceID { get; set; }
        public string AllowanceName { get; set; } = string.Empty;
        public Guid CategoryID { get; set; }
        public string Category { get; set; } = string.Empty;  
        public string? Description { get; set; }

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }


    }
}

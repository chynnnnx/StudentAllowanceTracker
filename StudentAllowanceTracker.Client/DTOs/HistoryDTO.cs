namespace StudentAllowanceTracker.Client.DTOs
{
    public class HistoryDTO
    {
        public Guid HistoryID { get; set; }
        public string UserID { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal? Amount { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
    }
}

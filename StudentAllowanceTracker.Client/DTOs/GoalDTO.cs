namespace StudentAllowanceTracker.Client.DTOs
{
    public class GoalDTO
    {
        public Guid GoalID { get; set; }
        public string UserID { get; set; } = string.Empty;

        public string GoalName { get; set; } = string.Empty;
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public GoalDTO Clone()
        {
            return (GoalDTO)this.MemberwiseClone();
        }

    }
}

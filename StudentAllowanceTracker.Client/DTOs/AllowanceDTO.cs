using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Client.DTOs
{
    public class AllowanceDTO
    {
        public Guid AllowanceID { get; set; }
        public string IdentityId { get; set; } = default!;

        public decimal Amount { get; set; }      
        public string? Description { get; set; }  
        
        public DateTime StartDate { get; set; }  
        public DateTime? EndDate { get; set; }

        public AllowanceType Type { get; set; }
    }
}

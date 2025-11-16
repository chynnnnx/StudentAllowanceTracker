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
        public override string ToString()
        {
            return $"₱{Amount:N2} — {StartDate:MMM dd, yyyy}";
        }

        public override bool Equals(object? obj)
        {
            return obj is AllowanceDTO dto && AllowanceID == dto.AllowanceID;
        }

        public override int GetHashCode() => AllowanceID.GetHashCode();
    
}
}

using StudentAllowanceTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.DTOs
{
    public class AllowanceDTO
    {
        public Guid AllowanceID { get; set; }
        public string Id { get; set; }
        public decimal Amount { get; set; }       
        public string? Description { get; set; }  

        public DateTime StartDate { get; set; }    
        public DateTime? EndDate { get; set; }   

        public AllowanceType Type { get; set; }
    }
}

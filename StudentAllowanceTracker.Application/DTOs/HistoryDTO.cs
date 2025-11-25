using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.DTOs
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

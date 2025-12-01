using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string TokenHash { get; set; } = default!; 
        public string UserID { get; set; } = default!;
        public DateTime Expiration { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}

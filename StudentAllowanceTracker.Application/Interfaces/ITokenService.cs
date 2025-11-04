using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StudentAllowanceTracker.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string email, string firstName, string lastName, IList<string> roles);
    }
}


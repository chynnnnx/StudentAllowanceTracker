using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StudentAllowanceTracker.Application.Interfaces
{
    public interface ITokenService
    {
        Task<AuthResult> GenerateTokensAsync(AppIdentityUser user, IList<string> roles);

        Task<AuthResult> RefreshTokensAsync(string rawToken);



    }
}


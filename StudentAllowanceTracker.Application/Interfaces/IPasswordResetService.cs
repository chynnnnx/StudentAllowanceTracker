using StudentAllowanceTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Interfaces
{
    public interface IPasswordResetService
    {
        Task SendResetCodeAsync(AppIdentityUser user, string email);
    }
}

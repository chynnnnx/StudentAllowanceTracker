using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Interfaces
{
    public interface IEmailVerificationCodeRepository : IBaseRepository<EmailVerificationCode>
    {

        Task<EmailVerificationCode?> GetByCodeAsync(string code);
        Task<List<EmailVerificationCode>> GetUnexpiredCodesAsync(string email);
        Task UpdateRangeAsync(IEnumerable<EmailVerificationCode> codes);
    }
}

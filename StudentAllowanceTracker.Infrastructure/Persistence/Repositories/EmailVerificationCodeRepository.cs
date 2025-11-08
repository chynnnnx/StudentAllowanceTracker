using Microsoft.EntityFrameworkCore;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Infrastructure.Persistence.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Infrastructure.Persistence.Repositories
{
    public class EmailVerificationCodeRepository
        : BaseRepository<EmailVerificationCode>, IEmailVerificationCodeRepository
    {
        private readonly AppDbContext _context;

        public EmailVerificationCodeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EmailVerificationCode?> GetByCodeAsync(string code)
        {
            return await _context.EmailVerificationCodes
                .FirstOrDefaultAsync(x => x.Code == code);
        }
        public async Task<List<EmailVerificationCode>> GetUnexpiredCodesAsync(string email)
        {
            return await _context.EmailVerificationCodes
                .Where(x => x.Email == email && !x.IsUsed && x.Expiration > DateTime.UtcNow)
                .ToListAsync();
        }
        public async Task UpdateRangeAsync(IEnumerable<EmailVerificationCode> codes)
        {
            _context.EmailVerificationCodes.UpdateRange(codes);
            await _context.SaveChangesAsync();
        }
        

    }
}

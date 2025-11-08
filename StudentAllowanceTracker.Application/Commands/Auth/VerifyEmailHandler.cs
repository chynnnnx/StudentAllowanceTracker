using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Shared.Responses;

namespace StudentAllowanceTracker.Application.Commands.Auth
{
    public class VerifyEmailHandler : IRequestHandler<VerifyEmailCommand, Result<string>>
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IEmailVerificationCodeRepository _codeRepo;

        public VerifyEmailHandler(
            UserManager<AppIdentityUser> userManager,
            IEmailVerificationCodeRepository codeRepo)
        {
            _userManager = userManager;
            _codeRepo = codeRepo;
        }

        public async Task<Result<string>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result<string>.Fail(ResultStatus.NotFound, "User not found.");

            var codeEntry = await _codeRepo.GetByCodeAsync(request.Code);
            if (codeEntry == null || codeEntry.Email != request.Email)
                return Result<string>.Fail(ResultStatus.Failed, "Invalid verification code.");

            if (codeEntry.IsUsed || codeEntry.Expiration < DateTime.UtcNow)
                return Result<string>.Fail(ResultStatus.Failed, "This code has expired or already been used.");

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            codeEntry.IsUsed = true;
            await _codeRepo.UpdateAsync(codeEntry);

            return Result<string>.Ok("Email verified successfully.");
        }
    }
}

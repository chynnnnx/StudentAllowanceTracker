using MediatR;
using Microsoft.AspNetCore.Identity;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Commands.Auth.PasswordRecovery
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<bool>>
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IEmailVerificationCodeRepository _codeRepo;
        public ResetPasswordCommandHandler(UserManager<AppIdentityUser> userManager, IEmailVerificationCodeRepository codeRepo)
        {
            _userManager = userManager;
            _codeRepo = codeRepo;
        }
        public async Task<Result<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.NewPassword != request.ConfirmPassword)
                return Result<bool>.Fail(ResultStatus.ValidationError, "Passwords do not match.");

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result<bool>.Fail(ResultStatus.NotFound, "User not found.");

            var codeEntry = await _codeRepo.GetByCodeAsync(request.Code);

            if (codeEntry == null || codeEntry.Email != request.Email)
                return Result<bool>.Fail(ResultStatus.Failed, "Invalid or expired code.");

            if (codeEntry.IsUsed || codeEntry.Expiration < DateTime.UtcNow)
                return Result<bool>.Fail(ResultStatus.Failed, "Code expired or already used.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (!result.Succeeded)
                return Result<bool>.Fail(ResultStatus.ValidationError, result.Errors.Select(e => e.Description));

            codeEntry.IsUsed = true;
            await _codeRepo.UpdateAsync(codeEntry);

            return Result<bool>.Ok(true);
        }
    }
}

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
    public class ResendVerificationHandler : IRequestHandler<ResendVerificationCommand, Result<string>>
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IEmailVerificationCodeRepository _codeRepo;
        private readonly ICodeGeneratorService _codeGenerator;
        private readonly IEmailService _emailService;

        public ResendVerificationHandler( UserManager<AppIdentityUser> userManager,IEmailVerificationCodeRepository codeRepo, ICodeGeneratorService codeGenerator,IEmailService emailService)
        {
            _userManager = userManager;
            _codeRepo = codeRepo;
            _codeGenerator = codeGenerator;
            _emailService = emailService;
        }

        public async Task<Result<string>> Handle(ResendVerificationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result<string>.Fail(ResultStatus.Failed, new[] { "Email not found." });

            if (user.EmailConfirmed)
                return Result<string>.Fail(ResultStatus.Failed, new[] { "Email is already verified." });

            var existingCodes = await _codeRepo.GetUnexpiredCodesAsync(user.Email!);
            foreach (var c in existingCodes)
            {
                c.IsUsed = true;
            }
            await _codeRepo.UpdateRangeAsync(existingCodes);

            var code = _codeGenerator.Generate(6);

            var verification = new EmailVerificationCode
            {
                UserId = user.Id,
                Email = user.Email!,
                Code = code,
                Expiration = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };

            await _codeRepo.AddAsync(verification);

            await _emailService.SendEmailAsync(
                user.Email!,
                "Your new verification code",
                $"<p>Your new verification code is <strong>{code}</strong>.</p><p>This code will expire in 10 minutes.</p>"
            );

            return Result<string>.Ok("Verification code sent successfully.");
        }

    }
}

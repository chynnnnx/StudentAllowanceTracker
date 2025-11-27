using MediatR;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using StudentAllowanceTracker.Application.Interfaces;
namespace StudentAllowanceTracker.Application.Commands.Auth
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<bool>>
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ICodeGeneratorService _codeGeneratorService;
        private readonly IEmailVerificationCodeRepository _codeRepo;

        public ForgotPasswordCommandHandler(UserManager<AppIdentityUser> userManager, IEmailService emailService, ICodeGeneratorService codeGeneratorService, IEmailVerificationCodeRepository codeRepo)
        {
            _userManager = userManager;
            _emailService = emailService;
            _codeGeneratorService = codeGeneratorService;
            _codeRepo = codeRepo;
        }
        public async Task<Result<bool>> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
            {
                return Result<bool>.Ok(true);
            }
            var code = _codeGeneratorService.Generate(6);
            var codeEntry = new EmailVerificationCode
            {
                UserId = user.Id,
                Email = command.Email,
                Code = code,
                Expiration = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };
            await _codeRepo.AddAsync(codeEntry);
            var emailBody = $"Your password reset code is: {code}. This code will expire in 10 minutes.";
            await _emailService.SendEmailAsync(command.Email, "Password Reset Code", emailBody);
            return Result<bool>.Ok(true);
        }
    }
}

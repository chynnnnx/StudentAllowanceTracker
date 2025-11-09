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
    public class RegisterHandler : IRequestHandler<RegisterCommand, Result<string>>
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailVerificationCodeRepository _codeRepo;
        private readonly ICodeGeneratorService _codeGenerator;
        private readonly IEmailService _emailService;

        public RegisterHandler(UserManager<AppIdentityUser> userManager, RoleManager<IdentityRole> roleManager, IEmailVerificationCodeRepository codeRepo, ICodeGeneratorService codeGenerator,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _codeRepo = codeRepo;
            _codeGenerator = codeGenerator;
            _emailService = emailService;
        }

        public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var identityUser = new AppIdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(identityUser, request.Password);
            if (!result.Succeeded)
                return Result<string>.Fail(ResultStatus.Failed, result.Errors.Select(e => e.Description));

            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(identityUser, "User");

            var code = _codeGenerator.Generate(6);

            var verification = new EmailVerificationCode
            {
                UserId = identityUser.Id,       
                Email = identityUser.Email!,
                Code = code,
                Expiration = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };

            await _codeRepo.AddAsync(verification);

                await _emailService.SendEmailAsync(
              identityUser.Email!,
              "Verify your email",
              $"<p>Your verification code is <strong>{code}</strong>.</p><p>This code will expire in 10 minutes.</p>"
          );

            return Result<string>.Ok(identityUser.Id);
        }
    }
}

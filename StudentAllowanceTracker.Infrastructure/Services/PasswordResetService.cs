using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Application.Interfaces.Repositories;

public class PasswordResetService : IPasswordResetService
{
    private readonly ICodeGeneratorService _codeGenerator;
    private readonly IEmailVerificationCodeRepository _codeRepo;
    private readonly IEmailService _emailService;

    public PasswordResetService(
        ICodeGeneratorService codeGenerator,
        IEmailVerificationCodeRepository codeRepo,
        IEmailService emailService)
    {
        _codeGenerator = codeGenerator;
        _codeRepo = codeRepo;
        _emailService = emailService;
    }

    public async Task SendResetCodeAsync(AppIdentityUser user, string email)
    {
        var code = _codeGenerator.Generate(6);

        var codeEntry = new EmailVerificationCode
        {
            UserId = user.Id,
            Email = email,
            Code = code,
            Expiration = DateTime.UtcNow.AddMinutes(10),
            IsUsed = false
        };
        await _codeRepo.AddAsync(codeEntry);


        var emailBody = $"Your password reset code is: {code}. This code will expire in 10 minutes.";
        await _emailService.SendEmailAsync(email, "Password Reset Code", emailBody);
    }
}

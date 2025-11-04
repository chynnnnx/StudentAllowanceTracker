using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Shared.Responses;

namespace StudentAllowanceTracker.Application.Commands.Auth
{
    public class LoginHandler : IRequestHandler<LoginCommand, Result<string>>
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly ITokenService _tokenService;

        public LoginHandler(UserManager<AppIdentityUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result<string>.Fail(ResultStatus.Failed, new[] { "Invalid email or password" });

            if (!user.EmailConfirmed)
                return Result<string>.Fail(ResultStatus.Failed, new[] { "Email not verified. Please check your inbox." });

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
                return Result<string>.Fail(ResultStatus.Failed, new[] { "Invalid email or password" });

            var roles = await _userManager.GetRolesAsync(user);

            var token = _tokenService.GenerateToken(
                user.Id,
                user.Email!,
                user.FirstName!,
                user.LastName!,
                roles
            );

            return Result<string>.Ok(token);
        }

    }
}

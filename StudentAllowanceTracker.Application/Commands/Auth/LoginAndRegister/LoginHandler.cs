using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Shared.Responses;

namespace StudentAllowanceTracker.Application.Commands.Auth.LoginAndRegister
{
    public class LoginHandler : IRequestHandler<LoginCommand, Result<AuthResult>>
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly ITokenService _tokenService;

        public LoginHandler(UserManager<AppIdentityUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<Result<AuthResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return Result<AuthResult>.Fail(ResultStatus.Failed, new[] { "Invalid email or password" });

            if (!user.EmailConfirmed)
                return Result<AuthResult>.Fail(ResultStatus.Failed, new[] { "Email not verified. Please check your inbox." });

            var roles = await _userManager.GetRolesAsync(user);
            var authResult = await _tokenService.GenerateTokensAsync(user, roles);

            return Result<AuthResult>.Ok(authResult);
        }
    }

}

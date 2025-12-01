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
namespace StudentAllowanceTracker.Application.Commands.Auth.PasswordRecovery
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<bool>>
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IPasswordResetService _passwordResetService;

        public ForgotPasswordCommandHandler( UserManager<AppIdentityUser> userManager, IPasswordResetService passwordResetService)
        {
            _userManager = userManager;
            _passwordResetService = passwordResetService;
        }
        public async Task<Result<bool>> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);

            if (user == null)
                return Result<bool>.Ok(true);

            await _passwordResetService.SendResetCodeAsync(user, command.Email);

            return Result<bool>.Ok(true);
        }
    }
}

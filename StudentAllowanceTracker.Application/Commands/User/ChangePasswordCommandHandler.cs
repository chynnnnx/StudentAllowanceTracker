using MediatR;
using Microsoft.AspNetCore.Identity;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Domain.Entities;

namespace StudentAllowanceTracker.Application.Commands.User
{
    public class ChangePasswordCommandHandler: IRequestHandler<ChangePasswordCommand, Result<bool>>
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly ICurrentUserService _currentUser;
        public ChangePasswordCommandHandler(UserManager<AppIdentityUser> userManager, ICurrentUserService currentUser)
        {
            _userManager = userManager;
            _currentUser = currentUser;
        }
        public async Task<Result<bool>> Handle (ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (userId == null)
            {
                return Result<bool>.Fail(ResultStatus.Unauthorized, "User not logged in.");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.Fail(ResultStatus.NotFound, "User not found.");
            }
            if (command.NewPassword != command.ConfirmPassword)
            {
                return Result<bool>.Fail(ResultStatus.ValidationError, "New password and confirmation do not match.");
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                var errors = changePasswordResult.Errors.Select(e => e.Description);
                return Result<bool>.Fail(ResultStatus.ValidationError, errors);
            }
            return Result<bool>.Ok(true);
        }


    }
}

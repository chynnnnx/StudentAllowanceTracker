using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Application.Interfaces;
using AutoMapper;
using StudentAllowanceTracker.Shared.Enums;
using Microsoft.Extensions.Logging.Abstractions;

namespace StudentAllowanceTracker.Application.Commands.User
{
    public class UpdateUserCommandHandler: IRequestHandler<UpdateUserCommand, Result<AuthUserDTO>>
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        public UpdateUserCommandHandler(UserManager<AppIdentityUser> userManager, IMapper mapper, ICurrentUserService currentUser)
        {
            _userManager = userManager;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task <Result<AuthUserDTO>> Handle (UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<AuthUserDTO>.Fail(ResultStatus.Unauthorized, "User not logged in.");

            var user = await _userManager.FindByIdAsync(command.Id);
            if (user == null)
                return Result<AuthUserDTO>.Fail(ResultStatus.NotFound, "User not found.");

            _mapper.Map(command, user);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Result<AuthUserDTO>.Fail(ResultStatus.Failed, "Failed to update user.");

            return Result<AuthUserDTO>.Ok(_mapper.Map<AuthUserDTO>(user));

        }
    }
}

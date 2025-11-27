using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using StudentAllowanceTracker.Domain.Entities;
using AutoMapper;


namespace StudentAllowanceTracker.Application.Queries.User
{
    public class GetInfoByUserQueryHandler: IRequestHandler<GetInfoByUserQuery,AuthUserDTO>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IMapper _mapper;
        public GetInfoByUserQueryHandler(ICurrentUserService currentUser, UserManager<AppIdentityUser> userManager, IMapper mapper)
        {
            _currentUser = currentUser;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AuthUserDTO> Handle (GetInfoByUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                return null;
           
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            return _mapper.Map<AuthUserDTO>(user);

        }
    }
}

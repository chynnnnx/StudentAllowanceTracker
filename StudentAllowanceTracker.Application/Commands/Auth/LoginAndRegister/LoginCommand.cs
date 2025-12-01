using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Shared.Responses;

namespace StudentAllowanceTracker.Application.Commands.Auth.LoginAndRegister
{
    public class LoginCommand : IRequest<Result<AuthResult>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

   
}


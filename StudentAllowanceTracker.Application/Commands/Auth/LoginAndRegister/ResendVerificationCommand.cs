using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using StudentAllowanceTracker.Shared.Responses;

namespace StudentAllowanceTracker.Application.Commands.Auth.LoginAndRegister
{
    public class ResendVerificationCommand : IRequest<Result<string>>
    {
        public string Email { get; set; } = null!;
    }
}

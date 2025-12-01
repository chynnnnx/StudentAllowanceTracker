using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using StudentAllowanceTracker.Shared.Responses;

namespace StudentAllowanceTracker.Application.Commands.Auth.PasswordRecovery
{
    public class ResetPasswordCommand : IRequest<Result<bool>>
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

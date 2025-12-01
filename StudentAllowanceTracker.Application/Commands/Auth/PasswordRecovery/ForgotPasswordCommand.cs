using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Shared.Responses;
using MediatR;
namespace StudentAllowanceTracker.Application.Commands.Auth.PasswordRecovery
{
    public class ForgotPasswordCommand: IRequest<Result<bool>>
    {
        public string Email { get; set; } = string.Empty;
    }
}

using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Shared.Responses;

namespace StudentAllowanceTracker.Application.Commands.Auth.RefreshTokens
{
    public class RefreshTokenCommand : IRequest<Result<AuthResult>>
    {
        public string RefreshToken { get; set; } = default!;
    }
}

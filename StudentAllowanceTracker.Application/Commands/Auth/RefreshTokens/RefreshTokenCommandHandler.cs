    using MediatR;
    using StudentAllowanceTracker.Application.DTOs;
    using StudentAllowanceTracker.Application.Interfaces.Repositories;
    using StudentAllowanceTracker.Application.Interfaces;
    using StudentAllowanceTracker.Shared.Enums;
    using StudentAllowanceTracker.Shared.Responses;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using StudentAllowanceTracker.Domain.Entities;
 
    namespace StudentAllowanceTracker.Application.Commands.Auth.RefreshTokens
    {
        public class RefreshTokenCommandHandler
         : IRequestHandler<RefreshTokenCommand, Result<AuthResult>>
        {
            private readonly ITokenService _tokenService;

            public RefreshTokenCommandHandler(ITokenService tokenService)
            {
                _tokenService = tokenService;
            }

            public async Task<Result<AuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var tokens = await _tokenService.RefreshTokensAsync(request.RefreshToken);
                    return Result<AuthResult>.Ok(tokens);
                }
                catch (Exception ex)
                {
                    return Result<AuthResult>.Fail(ResultStatus.Failed, new[] { ex.Message });
                }
            }
        }

    }

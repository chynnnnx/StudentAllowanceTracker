    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using StudentAllowanceTracker.Application.DTOs;
    using StudentAllowanceTracker.Application.Interfaces;
    using StudentAllowanceTracker.Application.Interfaces.Repositories;
    using StudentAllowanceTracker.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    namespace StudentAllowanceTracker.Infrastructure.Services
    {
        public class TokenService : ITokenService
        {
            private readonly IConfiguration _config;
            private readonly IBaseRepository<RefreshToken> _refreshTokenRepo;
            private readonly UserManager<AppIdentityUser> _userManager;

            public TokenService(
                IConfiguration config,
                IBaseRepository<RefreshToken> refreshTokenRepo,
                UserManager<AppIdentityUser> userManager)
            {
                _config = config;
                _refreshTokenRepo = refreshTokenRepo;
                _userManager = userManager;
            }
            public async Task<AuthResult> GenerateTokensAsync(AppIdentityUser user, IList<string> roles)
            {
                var accessToken = GenerateJwtToken(user, roles);

                var rawRefreshToken = GenerateRefreshToken();
                var hashed = Hash(rawRefreshToken);

                var refresh = new RefreshToken
                {
                    UserID = user.Id,
                    TokenHash = hashed,
                    Expiration = DateTime.UtcNow.AddDays(30),
                    CreatedAt = DateTime.UtcNow,
                    IsRevoked = false
                };

                await _refreshTokenRepo.AddAsync(refresh);
            
                return new AuthResult
                {
                    AccessToken = accessToken,
                    RefreshToken = rawRefreshToken
                };
            }
            public async Task<AuthResult> RefreshTokensAsync(string rawToken)
            {
                var hashed = Hash(rawToken);

                var token = await _refreshTokenRepo.FindOneAsync(
                    t => t.TokenHash == hashed && !t.IsRevoked
                );

                if (token == null)
                    throw new Exception("Invalid refresh token.");

                if (token.Expiration <= DateTime.UtcNow)
                    throw new Exception("Refresh token expired.");

                token.IsRevoked = true;
                await _refreshTokenRepo.UpdateAsync(token);

                var user = await _userManager.FindByIdAsync(token.UserID);
                var roles = await _userManager.GetRolesAsync(user);

                var newAccessToken = GenerateJwtToken(user, roles);
                var newRawToken = GenerateRefreshToken();
                var newHashed = Hash(newRawToken);

                var newRefresh = new RefreshToken
                {
                    UserID = user.Id,
                    TokenHash = newHashed,
                    Expiration = DateTime.UtcNow.AddDays(30),
                    CreatedAt = DateTime.UtcNow,
                    IsRevoked = false
                };

                await _refreshTokenRepo.AddAsync(newRefresh);

                return new AuthResult
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRawToken
                };
            }
            private string GenerateJwtToken(AppIdentityUser user, IList<string> roles)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email ?? "")
                };

                foreach (var role in roles)
                    claims.Add(new Claim(ClaimTypes.Role, role));

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            private string GenerateRefreshToken()
            {
                var bytes = new byte[64];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }

            private string Hash(string token)
            {
                using var sha = SHA256.Create();
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
                return Convert.ToBase64String(bytes);
            }
        }
    }

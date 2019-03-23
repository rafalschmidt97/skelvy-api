using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Skelvy.Application.Auth.Commands;
using Skelvy.Application.Infrastructure.Tokens;
using Skelvy.Domain.Entities;

namespace Skelvy.Infrastructure.Tokens
{
  public class TokenService : ITokenService
  {
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public string Generate(User user, AccessVerification verification)
    {
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Sid, user.Id.ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(AuthClaimTypes.Type, verification.AccessType),
        new Claim(AuthClaimTypes.UserId, verification.UserId),
        new Claim(AuthClaimTypes.AccessToken, verification.AccessToken)
      };

      var token = new JwtSecurityToken(
        _configuration["Jwt:Issuer"],
        null,
        claims.ToArray(),
        expires: DateTimeOffset.UtcNow.AddDays(7).UtcDateTime,
        signingCredentials: credentials);

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}

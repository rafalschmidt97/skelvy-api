using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Skelvy.Application.Auth.Commands;
using Skelvy.Application.Auth.Infrastructure.Repositories;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Core.Cache;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Infrastructure.Auth.Tokens
{
  public class TokenService : ITokenService
  {
    private readonly IConfiguration _configuration;
    private readonly IAuthRepository _authRepository;
    private readonly ICacheService _cache;
    private readonly IMapper _mapper;

    public TokenService(IConfiguration configuration, IAuthRepository authRepository, ICacheService cache, IMapper mapper)
    {
      _configuration = configuration;
      _authRepository = authRepository;
      _cache = cache;
      _mapper = mapper;
    }

    public async Task<AuthDto> Generate(User user)
    {
      var refreshToken = await GetRefreshToken(user);
      var accessToken = GetAccessToken(user);

      return new AuthDto { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    public async Task<AuthDto> Generate(string refreshToken)
    {
      var user = await UpdateRefreshToken(refreshToken);
      var accessToken = GetAccessToken(user);

      return new AuthDto { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    public async Task Invalidate(string refreshToken)
    {
      await _cache.RemoveData($"auth:refresh#{refreshToken}");
    }

    private string GetAccessToken(User user)
    {
      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Sid, user.Id.ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
      };

      claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
      return GenerateAccessToken(DateTimeOffset.UtcNow.AddMinutes(5).UtcDateTime, claims);
    }

    private async Task<string> GetRefreshToken(User user)
    {
      var refreshToken = GenerateRefreshToken();
      await _cache.SetData($"auth:refresh#{refreshToken}", TimeSpan.FromDays(15), _mapper.Map<TokenUser>(user));
      return refreshToken;
    }

    private async Task<User> UpdateRefreshToken(string refreshToken)
    {
      var key = $"auth:refresh#{refreshToken}";
      var tokenUser = await _cache.GetData<TokenUser>(key);

      if (tokenUser == null)
      {
        throw new UnauthorizedException("Refresh Token has expired");
      }

      var user = await _authRepository.FindOneWithRoles(tokenUser.Id);
      ValidateUser(user);
      await _cache.RefreshData(key);
      return user;
    }

    private string GenerateAccessToken(DateTime expires, List<Claim> claims = null)
    {
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SKELVY_JWT_KEY"]));
      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var rawAccessToken = new JwtSecurityToken(
        _configuration["SKELVY_JWT_ISSUER"],
        null,
        claims?.ToArray(),
        expires: expires,
        signingCredentials: credentials);

      return new JwtSecurityTokenHandler().WriteToken(rawAccessToken);
    }

    private static string GenerateRefreshToken()
    {
      var randomNumber = new byte[32];
      using (var rng = RandomNumberGenerator.Create())
      {
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
      }
    }

    private static void ValidateUser(User user)
    {
      if (user == null)
      {
        throw new UnauthorizedException("User from Refresh Token does not exists");
      }

      if (user.IsRemoved)
      {
        throw new UnauthorizedException("User is in safety retention window for deletion");
      }

      if (user.IsDisabled)
      {
        throw new UnauthorizedException("User is disabled");
      }
    }
  }
}

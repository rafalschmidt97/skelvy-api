using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Skelvy.Application.Auth.Commands;
using Skelvy.Application.Auth.Infrastructure.Repositories;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Common.Extensions;
using Skelvy.Domain.Entities;

namespace Skelvy.Infrastructure.Auth.Tokens
{
  public class TokenService : ITokenService
  {
    private readonly IConfiguration _configuration;
    private readonly IUsersRepository _usersRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public TokenService(IConfiguration configuration, IUsersRepository usersRepository, IRefreshTokenRepository refreshTokenRepository)
    {
      _configuration = configuration;
      _usersRepository = usersRepository;
      _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<TokenDto> Generate(User user)
    {
      var refreshToken = await GenerateRefreshTokenFromUser(user);
      var accessToken = GenerateAccessTokenFromUser(user);

      return new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    public async Task<TokenDto> Generate(string refreshToken)
    {
      var user = await GetUserFromRefreshToken(refreshToken);
      var accessToken = GenerateAccessTokenFromUser(user);

      return new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    public async Task Invalidate(string refreshToken)
    {
      var token = await _refreshTokenRepository.FindOneByToken(refreshToken);

      if (token == null)
      {
        throw new UnauthorizedException("Refresh Token does not exist");
      }

      await _refreshTokenRepository.Remove(token);
    }

    private string GenerateAccessTokenFromUser(User user)
    {
      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Sid, user.Id.ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
      };

      if (user.Email != null)
      {
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
      }

      user.Roles?.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role.Name)));

      return GenerateAccessToken(DateTimeOffset.UtcNow.AddMinutes(5).UtcDateTime, claims);
    }

    private async Task<string> GenerateRefreshTokenFromUser(User user)
    {
      var refreshToken = GenerateRefreshToken();
      var token = new RefreshToken(DateTimeOffset.UtcNow.AddYears(2), refreshToken, user.Id);
      await _refreshTokenRepository.Add(token);
      return refreshToken;
    }

    private async Task<User> GetUserFromRefreshToken(string refreshToken)
    {
      var token = await _refreshTokenRepository.FindOneByToken(refreshToken);

      if (token == null)
      {
        throw new UnauthorizedException("Refresh Token has expired");
      }

      var user = await _usersRepository.FindOneWithRoles(token.UserId);
      ValidateUser(user);
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
      using var rng = RandomNumberGenerator.Create();
      rng.GetBytes(randomNumber);
      return Convert.ToBase64String(randomNumber);
    }

    private static void ValidateUser(User user)
    {
      if (user == null)
      {
        throw new UnauthorizedException("User from Refresh Token does not exist");
      }

      if (user.IsRemoved)
      {
        throw new UnauthorizedException($"{nameof(User)}({user.Id}) is in safety retention window for deletion.");
      }

      if (user.IsDisabled)
      {
        throw new UnauthorizedException($"{nameof(User)}({user.Id}) has been disabled.");
      }
    }
  }
}

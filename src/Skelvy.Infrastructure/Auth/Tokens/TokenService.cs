using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Skelvy.Application.Auth.Commands;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Common.Exceptions;
using Skelvy.Common.Serializers;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Infrastructure.Auth.Tokens
{
  public class TokenService : ITokenService
  {
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;
    private readonly SkelvyContext _context;

    public TokenService(IConfiguration configuration, IDistributedCache cache, IMapper mapper, SkelvyContext context)
    {
      _configuration = configuration;
      _cache = cache;
      _mapper = mapper;
      _context = context;
    }

    public async Task<AuthDto> Generate(User user)
    {
      var refreshToken = await GetRefreshToken(user);
      var accessToken = GetAccessToken(user);

      return new AuthDto(accessToken, refreshToken);
    }

    public async Task<AuthDto> Generate(string refreshToken)
    {
      var user = await UpdateRefreshToken(refreshToken);
      var accessToken = GetAccessToken(user);

      return new AuthDto(accessToken, refreshToken);
    }

    public async Task Invalidate(string refreshToken)
    {
      var cacheKey = $"auth:refresh#{refreshToken}";
      await _cache.RemoveAsync(cacheKey);
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

      var cacheKey = $"auth:refresh#{refreshToken}";
      var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(15));
      await _cache.SetAsync(cacheKey, _mapper.Map<TokenUser>(user).Serialize(), options);

      return refreshToken;
    }

    private async Task<User> UpdateRefreshToken(string refreshToken)
    {
      var cacheKey = $"auth:refresh#{refreshToken}";
      var cachedBytes = await _cache.GetAsync(cacheKey);

      if (cachedBytes == null)
      {
        throw new UnauthorizedException("Refresh Token has expired");
      }

      var tokenUser = cachedBytes.Deserialize<TokenUser>();
      var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == tokenUser.Id);

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

      await _cache.RefreshAsync(cacheKey);
      return user;
    }

    private string GenerateAccessToken(DateTime expires, List<Claim> claims = null)
    {
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var rawAccessToken = new JwtSecurityToken(
        _configuration["Jwt:Issuer"],
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
  }
}

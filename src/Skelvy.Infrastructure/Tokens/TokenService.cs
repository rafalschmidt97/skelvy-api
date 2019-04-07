using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Skelvy.Application.Infrastructure.Tokens;
using Skelvy.Common.Exceptions;
using Skelvy.Common.Serializers;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Infrastructure.Tokens
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

    public async Task<Token> Generate(User user, CancellationToken cancellationToken)
    {
      var refreshToken = await GenerateRefreshToken(user, cancellationToken);
      var accessToken = GenerateAccessToken(user);

      return new Token
      {
        AccessToken = accessToken,
        RefreshToken = refreshToken
      };
    }

    public async Task<Token> Generate(string refreshToken, CancellationToken cancellationToken)
    {
      var user = await RefreshToken(refreshToken, cancellationToken);
      var accessToken = GenerateAccessToken(user);

      return new Token
      {
        AccessToken = accessToken,
        RefreshToken = refreshToken
      };
    }

    public async Task Invalidate(string refreshToken, CancellationToken cancellationToken)
    {
      var cacheKey = $"auth:refresh#{refreshToken}";
      await _cache.RemoveAsync(cacheKey, cancellationToken);
    }

    private string GenerateAccessToken(User user)
    {
      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Sid, user.Id.ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email)
      };

      claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
      return GenerateToken(DateTimeOffset.UtcNow.AddMinutes(5).UtcDateTime, claims);
    }

    private async Task<string> GenerateRefreshToken(User user, CancellationToken cancellationToken)
    {
      var refreshToken = GenerateToken(DateTimeOffset.UtcNow.AddDays(15).UtcDateTime);

      var cacheKey = $"auth:refresh#{refreshToken}";
      var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(15));
      await _cache.SetAsync(cacheKey, _mapper.Map<TokenUser>(user).Serialize(), options, cancellationToken);

      return refreshToken;
    }

    private async Task<User> RefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
      var cacheKey = $"auth:refresh#{refreshToken}";
      var cachedBytes = await _cache.GetAsync(cacheKey, cancellationToken);

      if (cachedBytes == null)
      {
        throw new UnauthorizedException("Refresh Token has expired");
      }

      var tokenUser = cachedBytes.Deserialize<TokenUser>();
      var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == tokenUser.Id, cancellationToken);

      if (user == null)
      {
        throw new UnauthorizedException("User from Refresh Token does not exists");
      }

      await _cache.RefreshAsync(cacheKey, cancellationToken);
      return user;
    }

    private string GenerateToken(DateTime expires, List<Claim> claims = null)
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
  }
}

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Skelvy.Application.Auth.Commands;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Core.Infrastructure.Facebook;

namespace Skelvy.Infrastructure.Facebook
{
  public class FacebookService : HttpServiceBase, IFacebookService
  {
    private readonly string _clientId;
    private readonly string _clientSecret;

    public FacebookService(IConfiguration configuration)
      : base("https://graph.facebook.com/")
    {
      _clientId = configuration["Facebook:Id"];
      _clientSecret = configuration["Facebook:Secret"];
    }

    public async Task<T> GetBody<T>(string path, string accessToken, string args = null)
    {
      var response = await HttpClient.GetAsync($"{path}?access_token={accessToken}&{args}");
      if (!response.IsSuccessStatusCode)
      {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
          throw new UnauthorizedException("Facebook Token is not valid.");
        }

        throw new ConflictException($"Facebook GET problem with entity {nameof(T)}({path}?{args}).");
      }

      return await GetData<T>(response.Content);
    }

    public async Task<T> PostBody<T>(string path, string accessToken, object data, string args = null)
    {
      var response = await HttpClient.PostAsync($"{path}?access_token={accessToken}&{args}", PrepareData(data));

      if (!response.IsSuccessStatusCode)
      {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
          throw new UnauthorizedException("Facebook Token is not valid.");
        }

        throw new ConflictException($"Facebook POST problem with entity {nameof(T)}({path}?{args}).");
      }

      return await GetData<T>(response.Content);
    }

    public async Task<AccessVerification> Verify(string accessToken)
    {
      var response =
        await GetBody<dynamic>("debug_token", $"{_clientId}|{_clientSecret}", $"input_token={accessToken}");

      if (response.error != null)
      {
        throw new UnauthorizedException(response.error.message);
      }

      if (response.data.is_valid != true)
      {
        if (response.data.error != null && response.data.error.message != null)
        {
          throw new UnauthorizedException(response.data.error.message);
        }

        throw new UnauthorizedException("Facebook Token is not valid.");
      }

      return new AccessVerification
      {
        UserId = response.data.user_id,
        AccessToken = accessToken,
        ExpiresAt = UnixTimestampToDateTime(response.data.expires_at),
        IssuedAt = UnixTimestampToDateTime(response.data.issued_at),
        AccessType = AccessTypes.Facebook
      };
    }

    private static DateTime UnixTimestampToDateTime(dynamic unixTime)
    {
      var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
      var unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
      return new DateTime(unixStart.Ticks + unixTimeStampInTicks, DateTimeKind.Utc);
    }
  }
}

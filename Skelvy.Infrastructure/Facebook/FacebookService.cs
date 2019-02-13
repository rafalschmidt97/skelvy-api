using System;
using System.Net;
using System.Net.Http;
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
      var responseData = await GetData<T>(response.Content);

      ValidateResponse(path, args, response, responseData, "GET");

      return responseData;
    }

    public async Task<T> PostBody<T>(string path, string accessToken, object data, string args = null)
    {
      var response = await HttpClient.PostAsync($"{path}?access_token={accessToken}&{args}", PrepareData(data));
      var responseData = await GetData<T>(response.Content);

      ValidateResponse(path, args, response, responseData, "POST");

      return responseData;
    }

    public async Task<AccessVerification> Verify(string accessToken)
    {
      var response =
        await GetBody<dynamic>("debug_token", $"{_clientId}|{_clientSecret}", $"input_token={accessToken}");

      if (response.data.is_valid != true)
      {
        if (response.data.error != null && response.data.error.message != null)
        {
          throw new UnauthorizedException((string)response.data.error.message);
        }

        throw new UnauthorizedException("Facebook Token is not valid.");
      }

      return new AccessVerification
      {
        UserId = response.data.user_id,
        AccessToken = accessToken,
        ExpiresAt = UnixTimestampToDateTime(response.data.expires_at),
        AccessType = AccessTypes.Facebook
      };
    }

    private static DateTime UnixTimestampToDateTime(dynamic unixTime)
    {
      var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
      var unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
      return new DateTime(unixStart.Ticks + unixTimeStampInTicks, DateTimeKind.Utc);
    }

    private static void ValidateResponse<T>(
      string path,
      string args,
      HttpResponseMessage response,
      T responseData,
      string requestType)
    {
      if (response.IsSuccessStatusCode)
      {
        var responseDataDynamic = (dynamic)responseData;
        if (responseDataDynamic.error != null)
        {
          throw new BadRequestException((string)responseDataDynamic.error.message);
        }
      }
      else
      {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
          throw new UnauthorizedException("Facebook Token is not valid.");
        }

        throw new ConflictException($"Facebook {requestType} problem with entity {nameof(T)}({path}?{args}).");
      }
    }
  }
}

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Skelvy.Application.Auth.Commands;
using Skelvy.Application.Auth.Infrastructure.Google;
using Skelvy.Common.Exceptions;

namespace Skelvy.Infrastructure.Auth.Google
{
  public class GoogleService : HttpServiceBase, IGoogleService
  {
    private readonly string _idIos;
    private readonly string _idAndroid;

    public GoogleService(IConfiguration configuration)
      : base("https://www.googleapis.com/")
    {
      _idIos = configuration["SKELVY_GOOGLE_ID_IOS"];
      _idAndroid = configuration["SKELVY_GOOGLE_ID_ANDROID"];
    }

    public async Task<T> GetBody<T>(string path, string accessToken, string args)
    {
      var response = await HttpClient.GetAsync($"{path}?access_token={accessToken}&{args}");
      var responseData = await GetData<T>(response.Content);

      ValidateResponse(path, args, response, responseData, "GET");

      return responseData;
    }

    public async Task<T> PostBody<T>(string path, string accessToken, object data, string args)
    {
      var response = await HttpClient.PostAsync($"{path}?access_token={accessToken}&{args}", PrepareData(data));
      var responseData = await GetData<T>(response.Content);

      ValidateResponse(path, args, response, responseData, "POST");

      return responseData;
    }

    public async Task<AccessVerification> Verify(string accessToken)
    {
      var response =
        await GetBody<dynamic>("oauth2/v3/tokeninfo", accessToken, null);

      if (response.aud != _idIos && response.aud != _idAndroid)
      {
        throw new UnauthorizedException("Google Token Client is not valid.");
      }

      return new AccessVerification(
        (string)response.sub,
        accessToken,
        UnixTimestampToDateTime(response.exp),
        AccessType.Google);
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
      if (!response.IsSuccessStatusCode)
      {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
          throw new UnauthorizedException("Google Token is not valid.");
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
          var responseDataDynamic = (dynamic)responseData;
          if (responseDataDynamic.error_description != null)
          {
            throw new BadRequestException((string)responseDataDynamic.error_description);
          }
        }

        throw new ConflictException($"Google {requestType} problem with entity {nameof(T)}({path}?{args}).");
      }
    }
  }
}

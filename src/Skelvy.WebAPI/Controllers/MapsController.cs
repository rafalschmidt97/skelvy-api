using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Infrastructure.Maps;
using Skelvy.Application.Users.Commands;
using Skelvy.Common.Exceptions;
using Skelvy.Common.Serializers;

namespace Skelvy.WebAPI.Controllers
{
  public class MapsController : BaseController
  {
    private readonly IMapsService _mapsService;
    private readonly ILogger<MapsController> _logger;
    private readonly IDistributedCache _cache;

    public MapsController(IMapsService mapsService, ILogger<MapsController> logger, IDistributedCache cache)
    {
      _mapsService = mapsService;
      _logger = logger;
      _cache = cache;
    }

    [HttpGet("search")]
    public async Task<ICollection<Location>> Search(string search, string language = LanguageTypes.EN)
    {
      _logger.LogInformation("Request: Maps Search {search}", search);

      if (string.IsNullOrEmpty(search))
      {
        throw new BadRequestException("'search' must not be empty.");
      }

      if (!(language == LanguageTypes.EN || language == LanguageTypes.PL))
      {
        throw new BadRequestException($"'language' must be {LanguageTypes.PL} or {LanguageTypes.EN}");
      }

      var cacheKey = $"maps:search#{search}#{language}";
      var cachedLocationBytes = await _cache.GetAsync(cacheKey, HttpContext.RequestAborted);

      if (cachedLocationBytes != null)
      {
        return cachedLocationBytes.Deserialize<ICollection<Location>>();
      }

      try
      {
        var location = await _mapsService.Search(search, language, HttpContext.RequestAborted);
        var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(7));
        await _cache.SetAsync(cacheKey, location.Serialize(), options, HttpContext.RequestAborted);
        return location;
      }
      catch (CustomException exception)
      {
        _logger.LogError("Request {Status}: {Message}", exception.Status, exception.Message);
        throw;
      }
    }

    [HttpGet("reverse")]
    public async Task<ICollection<Location>> Reverse(
      double latitude,
      double longitude,
      string language = LanguageTypes.EN)
    {
      _logger.LogInformation(
        "Request: Maps Reverse (latitude: {latitude}, longitude: {longitude})",
        latitude,
        longitude);

      if (!(language == LanguageTypes.EN || language == LanguageTypes.PL))
      {
        throw new BadRequestException($"'language' must be {LanguageTypes.PL} or {LanguageTypes.EN}");
      }

      var cacheKey = $"maps:reverse#{latitude}#{longitude}#{language}";
      var cachedLocationBytes = await _cache.GetAsync(cacheKey, HttpContext.RequestAborted);

      if (cachedLocationBytes != null)
      {
        return cachedLocationBytes.Deserialize<ICollection<Location>>();
      }

      try
      {
        var location = await _mapsService.Search(latitude, longitude, language, HttpContext.RequestAborted);
        var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(7));
        await _cache.SetAsync(cacheKey, location.Serialize(), options, HttpContext.RequestAborted);
        return location;
      }
      catch (CustomException exception)
      {
        _logger.LogError("Request {Status}: {Message}", exception.Status, exception.Message);
        throw;
      }
    }
  }
}

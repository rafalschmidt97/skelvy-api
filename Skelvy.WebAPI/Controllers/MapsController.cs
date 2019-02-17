using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Core.Infrastructure.Maps;
using Skelvy.Common;

namespace Skelvy.WebAPI.Controllers
{
  [ResponseCache(Duration = 60 * 60 * 24 * 7)] // 7 days; TODO: It should at least log request
  public class MapsController : BaseController
  {
    private readonly IMapsService _mapsService;
    private readonly ILogger<MapsController> _logger;

    public MapsController(IMapsService mapsService, ILogger<MapsController> logger)
    {
      _mapsService = mapsService;
      _logger = logger;
    }

    [HttpGet("search")]
    public async Task<ICollection<Location>> Search(string search, string language = LanguageTypes.EN)
    {
      _logger.LogInformation("Request: Maps Search {search}", search);

      if (string.IsNullOrEmpty(search))
      {
        throw new BadRequestException("'search' must not be empty.");
      }

      try
      {
        return await _mapsService.Search(search, language);
      }
      catch (CustomException exception)
      {
        _logger.LogError("Request {Status}: {Message}", exception.Status, exception.Message);
        throw;
      }
    }

    [HttpGet("reverse")]
    public async Task<ICollection<Location>> Reverse(double latitude, double longitude, string language = LanguageTypes.EN)
    {
      _logger.LogInformation(
        "Request: Maps Reverse (latitude: {latitude}, longitude: {longitude})",
        latitude,
        longitude);

      try
      {
        return await _mapsService.Search(latitude, longitude, language);
      }
      catch (CustomException exception)
      {
        _logger.LogError("Request {Status}: {Message}", exception.Status, exception.Message);
        throw;
      }
    }
  }
}

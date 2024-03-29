using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geocoding.Google;
using Microsoft.Extensions.Configuration;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;

namespace Skelvy.Infrastructure.Maps.GoogleMaps
{
  public class MapsService : IMapsService
  {
    private readonly GoogleGeocoder _geocoder;

    public MapsService(IConfiguration configuration)
    {
      _geocoder = new GoogleGeocoder { ApiKey = configuration["SKELVY_GOOGLE_KEY_WEB"] };
    }

    public async Task<IList<LocationDto>> Search(string search, string language)
    {
      _geocoder.Language = language;
      var response = await _geocoder.GeocodeAsync(search);
      var filteredResponse = FilterAddresses(response);
      return MapToLocations(filteredResponse);
    }

    public async Task<IList<LocationDto>> Search(double latitude, double longitude, string language)
    {
      _geocoder.Language = language;
      var response = await _geocoder.ReverseGeocodeAsync(latitude, longitude);
      var filteredResponse = FilterAddresses(response);
      return MapToLocations(filteredResponse);
    }

    private static IEnumerable<GoogleAddress> FilterAddresses(IEnumerable<GoogleAddress> response)
    {
      return response
        .Where(locality =>
          locality.Type == GoogleAddressType.Locality ||
          locality.Type == GoogleAddressType.AdministrativeAreaLevel3);
    }

    private static IList<LocationDto> MapToLocations(IEnumerable<GoogleAddress> response)
    {
      return response.Select(MapToLocation).ToList();
    }

    private static LocationDto MapToLocation(GoogleAddress address)
    {
      var location = new LocationDto();
      string administrativeAreaLevel3 = null;

      foreach (var component in address.Components)
      {
        switch (component.Types[0])
        {
          case GoogleAddressType.Locality:
            location.City = component.LongName;
            break;
          case GoogleAddressType.AdministrativeAreaLevel3:
            administrativeAreaLevel3 = component.LongName;
            break;
          case GoogleAddressType.AdministrativeAreaLevel2:
            location.District = component.LongName;
            break;
          case GoogleAddressType.AdministrativeAreaLevel1:
            location.State = component.LongName;
            break;
          case GoogleAddressType.Country:
            location.Country = component.LongName;
            break;
        }
      }

      location.Latitude = address.Coordinates.Latitude;
      location.Longitude = address.Coordinates.Longitude;
      location.Type = address.Type.ToString();

      if (location.City == null)
      {
        location.City = administrativeAreaLevel3;
      }

      return location;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Core.Cache;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;
using Skelvy.Application.Maps.Queries.SearchLocations;
using Skelvy.Domain.Enums.Users;
using Xunit;

namespace Skelvy.Application.Test.Maps.Queries
{
  public class SearchLocationsQueryHandlerTest : RequestTestBase
  {
    private readonly Mock<IMapsService> _mapsService;
    private readonly Mock<ICacheService> _cache;

    public SearchLocationsQueryHandlerTest()
    {
      _mapsService = new Mock<IMapsService>();
      _cache = new Mock<ICacheService>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new SearchLocationsQuery("Warsaw", LanguageTypes.EN);
      _cache.Setup(x => x.GetOrSetData(It.IsAny<string>(), It.IsAny<TimeSpan>(), It.IsAny<Func<Task<IList<LocationDto>>>>()))
        .ReturnsAsync(new List<LocationDto>());
      var handler = new SearchLocationsQueryHandler(_mapsService.Object, _cache.Object);

      await handler.Handle(request);
    }
  }
}

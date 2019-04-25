using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Core.Cache;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;
using Skelvy.Application.Maps.Queries.ReverseLocations;
using Skelvy.Domain.Enums.Users;
using Xunit;

namespace Skelvy.Application.Test.Maps.Queries
{
  public class ReverseLocationsQueryHandlerTest : RequestTestBase
  {
    private readonly Mock<IMapsService> _mapsService;
    private readonly Mock<ICacheService> _cache;

    public ReverseLocationsQueryHandlerTest()
    {
      _mapsService = new Mock<IMapsService>();
      _cache = new Mock<ICacheService>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new ReverseLocationsQuery(1, 1, LanguageTypes.EN);
      _cache.Setup(x => x.GetOrSetData(It.IsAny<string>(), It.IsAny<TimeSpan>(), It.IsAny<Func<Task<IList<LocationDto>>>>()))
        .ReturnsAsync(new List<LocationDto>());

      var handler = new ReverseLocationsQueryHandler(_mapsService.Object, _cache.Object);

      await handler.Handle(request);
    }
  }
}

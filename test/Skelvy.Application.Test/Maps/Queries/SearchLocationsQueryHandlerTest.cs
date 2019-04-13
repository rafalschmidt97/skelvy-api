using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;
using Skelvy.Application.Maps.Queries.SearchLocations;
using Skelvy.Application.Users.Commands;
using Xunit;

namespace Skelvy.Application.Test.Maps.Queries
{
  public class SearchLocationsQueryHandlerTest : RequestTestBase
  {
    private readonly Mock<IMapsService> _mapsService;
    private readonly Mock<IDistributedCache> _cache;

    public SearchLocationsQueryHandlerTest()
    {
      _mapsService = new Mock<IMapsService>();
      _cache = new Mock<IDistributedCache>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new SearchLocationsQuery { Search = "Warsaw", Language = LanguageTypes.EN };
      _cache.Setup(x => x.GetAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((byte[])null);
      var handler = new SearchLocationsQueryHandler(_mapsService.Object, _cache.Object);

      await handler.Handle(request);
    }
  }
}

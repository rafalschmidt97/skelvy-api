using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Skelvy.Application.Infrastructure.Maps;
using Skelvy.Application.Maps.Queries.ReverseLocations;
using Skelvy.Application.Users.Commands;
using Xunit;

namespace Skelvy.Application.Test.Maps.Queries
{
  public class ReverseLocationsQueryHandlerTest : RequestTestBase
  {
    private readonly Mock<IMapsService> _mapsService;
    private readonly Mock<IDistributedCache> _cache;

    public ReverseLocationsQueryHandlerTest()
    {
      _mapsService = new Mock<IMapsService>();
      _cache = new Mock<IDistributedCache>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new ReverseLocationsQuery { Latitude = 1, Longitude = 1, Language = LanguageTypes.EN };
      _cache.Setup(x => x.GetAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((byte[])null);
      var handler = new ReverseLocationsQueryHandler(_mapsService.Object, _cache.Object);

      await handler.Handle(request, CancellationToken.None);
    }
  }
}

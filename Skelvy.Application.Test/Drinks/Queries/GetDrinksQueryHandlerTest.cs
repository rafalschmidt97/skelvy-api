using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Drinks.Queries;
using Skelvy.Application.Drinks.Queries.GetDrinks;
using Xunit;

namespace Skelvy.Application.Test.Drinks.Queries
{
  public class GetDrinksQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnDrinks()
    {
      var request = new GetDrinksQuery();
      var handler = new GetDrinksQueryHandler(InitializedDbContext(), Mapper());

      var result = await handler.Handle(request, CancellationToken.None);

      Assert.All(result, x => Assert.IsType<DrinkDto>(x));
      Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ShouldReturnEmpty()
    {
      var request = new GetDrinksQuery();
      var handler = new GetDrinksQueryHandler(DbContext(), Mapper());

      var result = await handler.Handle(request, CancellationToken.None);

      Assert.All(result, x => Assert.IsType<DrinkDto>(x));
      Assert.Empty(result);
    }
  }
}

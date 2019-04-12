using System.Threading.Tasks;
using Skelvy.Application.Drinks.Queries;
using Skelvy.Application.Drinks.Queries.FindDrinks;
using Xunit;

namespace Skelvy.Application.Test.Drinks.Queries
{
  public class FindDrinksQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnDrinks()
    {
      var request = new FindDrinksQuery();
      var handler = new FindDrinksQueryHandler(InitializedDbContext(), Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<DrinkDto>(x));
      Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ShouldReturnEmpty()
    {
      var request = new FindDrinksQuery();
      var handler = new FindDrinksQueryHandler(DbContext(), Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<DrinkDto>(x));
      Assert.Empty(result);
    }
  }
}

using System.Threading.Tasks;
using Skelvy.Application.Drinks.Queries;
using Skelvy.Application.Drinks.Queries.FindDrinkTypes;
using Xunit;

namespace Skelvy.Application.Test.Drinks.Queries
{
  public class FindDrinkTypesQueryHandlerTest : DatabaseRequestTestBase
  {
    [Fact]
    public async Task ShouldReturnDrinkTypes()
    {
      var request = new FindDrinkTypesQuery();
      var handler = new FindDrinkTypesQueryHandler(DrinkTypesRepository(), Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<DrinkTypeDto>(x));
      Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ShouldReturnEmpty()
    {
      var request = new FindDrinkTypesQuery();
      var handler = new FindDrinkTypesQueryHandler(DrinkTypesRepository(false), Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<DrinkTypeDto>(x));
      Assert.Empty(result);
    }
  }
}

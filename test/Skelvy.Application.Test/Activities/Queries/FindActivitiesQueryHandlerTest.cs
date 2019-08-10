using System.Threading.Tasks;
using Skelvy.Application.Activities.Queries;
using Skelvy.Application.Activities.Queries.FindActivities;
using Xunit;

namespace Skelvy.Application.Test.Activities.Queries
{
  public class FindActivitiesQueryHandlerTest : DatabaseRequestTestBase
  {
    [Fact]
    public async Task ShouldReturnActivities()
    {
      var request = new FindActivitiesQuery();
      var handler = new FindActivitiesQueryHandler(ActivitiesRepository(), Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<ActivityDto>(x));
      Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ShouldReturnEmpty()
    {
      var request = new FindActivitiesQuery();
      var handler = new FindActivitiesQueryHandler(ActivitiesRepository(false), Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<ActivityDto>(x));
      Assert.Empty(result);
    }
  }
}

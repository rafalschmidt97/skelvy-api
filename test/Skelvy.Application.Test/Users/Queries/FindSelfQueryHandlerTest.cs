using System.Threading.Tasks;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.FindSelf;
using Skelvy.Common.Exceptions;
using Xunit;

namespace Skelvy.Application.Test.Users.Queries
{
  public class FindSelfQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnModel()
    {
      var request = new FindSelfQuery(1);
      var handler = new FindSelfQueryHandler(InitializedDbContext(), Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<SelfModel>(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindSelfQuery(1);
      var handler = new FindSelfQueryHandler(DbContext(), Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

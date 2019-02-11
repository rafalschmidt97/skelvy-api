using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.GetUser;
using Xunit;

namespace Skelvy.Application.Test.Users.Queries
{
  public class GetUserQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnUser()
    {
      var request = new GetUserQuery { Id = 1 };
      var handler = new GetUserQueryHandler(InitializedDbContext(), Mapper());

      var result = await handler.Handle(request, CancellationToken.None);

      Assert.IsType<UserDto>(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new GetUserQuery { Id = 2 };
      var handler = new GetUserQueryHandler(InitializedDbContext(), Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }
  }
}

using System.Threading.Tasks;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.FindUser;
using Skelvy.Common.Exceptions;
using Xunit;

namespace Skelvy.Application.Test.Users.Queries
{
  public class FindUserQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnUser()
    {
      var request = new FindUserQuery { Id = 1 };
      var handler = new FindUserQueryHandler(InitializedDbContext(), Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<UserDto>(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindUserQuery { Id = 1 };
      var handler = new FindUserQueryHandler(DbContext(), Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

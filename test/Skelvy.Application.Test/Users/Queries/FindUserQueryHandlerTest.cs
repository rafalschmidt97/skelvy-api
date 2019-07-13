using System.Threading.Tasks;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.FindUser;
using Skelvy.Common.Exceptions;
using Xunit;

namespace Skelvy.Application.Test.Users.Queries
{
  public class FindUserQueryHandlerTest : DatabaseRequestTestBase
  {
    [Fact]
    public async Task ShouldReturnUser()
    {
      var request = new FindUserQuery(1);
      var handler = new FindUserQueryHandler(UsersRepository(), Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<UserDto>(result);
      Assert.NotEqual(default(int), result.Profile.Age);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindUserQuery(1);
      var handler = new FindUserQueryHandler(UsersRepository(false), Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

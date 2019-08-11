using System.Threading.Tasks;
using Skelvy.Application.Users.Queries.CheckUserName;
using Xunit;

namespace Skelvy.Application.Test.Users.Queries
{
  public class CheckUserNameQueryHandlerTest : DatabaseRequestTestBase
  {
    [Fact]
    public async Task ShouldReturnIsAvailable()
    {
      var request = new CheckUserNameQuery("example");
      var handler = new CheckUserNameQueryHandler(UsersRepository());

      var result = await handler.Handle(request);

      Assert.IsType<bool>(result);
      Assert.True(result);
    }

    [Fact]
    public async Task ShouldReturnIsNotAvailable()
    {
      var request = new CheckUserNameQuery("user.1");
      var handler = new CheckUserNameQueryHandler(UsersRepository());

      var result = await handler.Handle(request);

      Assert.IsType<bool>(result);
      Assert.False(result);
    }
  }
}

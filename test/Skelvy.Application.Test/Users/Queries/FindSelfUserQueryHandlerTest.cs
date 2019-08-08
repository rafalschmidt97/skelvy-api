using System.Threading.Tasks;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.FindSelfUser;
using Skelvy.Common.Exceptions;
using Xunit;

namespace Skelvy.Application.Test.Users.Queries
{
  public class FindSelfUserQueryHandlerTest : DatabaseRequestTestBase
  {
    [Fact]
    public async Task ShouldReturnUser()
    {
      var request = new FindSelfUserQuery(1);
      var handler = new FindSelfUserQueryHandler(UsersRepository(), Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<SelfUserDto>(result);
      Assert.NotEqual(default, result.Profile.Birthday);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindSelfUserQuery(1);
      var handler = new FindSelfUserQueryHandler(UsersRepository(false), Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.GetUserDetail;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;
using Xunit;

namespace Skelvy.Application.Test.Users.Queries
{
  public class GetUserDetailQueryHandlerTest : RequestTestBase
  {
    private const string UserEmail = "user@gmail.com";
    private const string UserName = "User";

    [Fact]
    public async Task ShouldReturnUser()
    {
      var request = new GetUserDetailQuery { Id = 1 };
      var handler = new GetUserDetailQueryHandler(InitializedDbContext(), Mapper());

      var result = await handler.Handle(request, CancellationToken.None);

      Assert.IsType<UserDto>(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new GetUserDetailQuery { Id = 2 };
      var handler = new GetUserDetailQueryHandler(InitializedDbContext(), Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    private static SkelvyContext InitializedDbContext()
    {
      var context = DbContext();

      context.Users.Add(
        new User
        {
          Id = 1,
          Email = UserEmail,
          Name = UserName
        });

      context.SaveChanges();

      return context;
    }
  }
}

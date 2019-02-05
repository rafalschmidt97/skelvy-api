using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.GetUsers;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;
using Xunit;

namespace Skelvy.Application.Test.Users.Queries
{
  public class GetUsersQueryHandlerTest : RequestTestBase
  {
    private const string UserEmail = "user@gmail.com";
    private const string UserName = "User";

    [Fact]
    public async Task ShouldReturnUsers()
    {
      var request = new GetUsersQuery();
      var handler = new GetUsersQueryHandler(InitializedDbContext(), Mapper());

      var result = await handler.Handle(request, CancellationToken.None);

      Assert.NotEmpty(result);
      Assert.Collection(result, item => Assert.IsType<UserDto>(item));
    }

    private static SkelvyContext InitializedDbContext()
    {
      var context = DbContext();

      context.Users.Add(
        new User
        {
          Email = UserEmail,
          Name = UserName
        });

      context.SaveChanges();

      return context;
    }
  }
}

using System.Threading.Tasks;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.FIndUsers;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Users.Queries
{
  public class FindUsersQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnUsers()
    {
      var request = new FindUsersQuery(2, "user", 1);
      var dbContext = InitializedDbContext();

      var handler = new FindUsersQueryHandler(
        new UsersRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<UserWithRelationTypeDto>(x));
      Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ShouldReturnEmpty()
    {
      var request = new FindUsersQuery(2, "example", 1);
      var dbContext = InitializedDbContext();

      var handler = new FindUsersQueryHandler(
        new UsersRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<UserWithRelationTypeDto>(x));
      Assert.Empty(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindUsersQuery(2, "user", 1);
      var dbContext = DbContext();

      var handler = new FindUsersQueryHandler(
        new UsersRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

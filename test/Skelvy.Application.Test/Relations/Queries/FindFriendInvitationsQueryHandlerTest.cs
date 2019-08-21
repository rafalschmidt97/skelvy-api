using System.Threading.Tasks;
using Skelvy.Application.Relations.Queries;
using Skelvy.Application.Relations.Queries.FindFriendInvitations;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Relations.Queries
{
  public class FindFriendInvitationsQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnFriends()
    {
      var request = new FindFriendInvitationsQuery(2);
      var dbContext = InitializedDbContext();

      var handler = new FindFriendInvitationsQueryHandler(
        new FriendInvitationsRepository(dbContext),
        new UsersRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<FriendInvitationsDto>(x));
      Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ShouldReturnEmpty()
    {
      var request = new FindFriendInvitationsQuery(1);
      var dbContext = InitializedDbContext();

      var handler = new FindFriendInvitationsQueryHandler(
        new FriendInvitationsRepository(dbContext),
        new UsersRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<FriendInvitationsDto>(x));
      Assert.Empty(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindFriendInvitationsQuery(1);
      var dbContext = DbContext();

      var handler = new FindFriendInvitationsQueryHandler(
        new FriendInvitationsRepository(dbContext),
        new UsersRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

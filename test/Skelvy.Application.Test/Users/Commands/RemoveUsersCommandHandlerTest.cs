using System.Threading.Tasks;
using Skelvy.Application.Users.Commands.RemoveUsers;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class RemoveUsersCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveUsersCommand();
      var dbContext = InitializedDbContext();
      var handler = new RemoveUsersCommandHandler(
        new UsersRepository(dbContext),
        new UserRolesRepository(dbContext),
        new UserProfilesRepository(dbContext),
        new UserProfilePhotosRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestDrinksRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        new MeetingChatMessagesRepository(dbContext),
        new BlockedUsersRepository(dbContext));

      await handler.Handle(request);
    }
  }
}

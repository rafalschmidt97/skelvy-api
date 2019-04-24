using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Application.Users.Commands.RemoveUsers;
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
        new AuthRolesRepository(dbContext),
        new UserProfilesRepository(dbContext),
        new UserProfilePhotosRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestDrinksRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        new MeetingChatMessagesRepository(dbContext));

      await handler.Handle(request);
    }
  }
}

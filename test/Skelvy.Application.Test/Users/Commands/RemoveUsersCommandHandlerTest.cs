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
        new RefreshTokenRepository(dbContext),
        new ProfilesRepository(dbContext),
        new ProfilePhotosRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestActivityRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MessagesRepository(dbContext),
        new AttachmentsRepository(dbContext),
        new RelationsRepository(dbContext),
        new FriendInvitationsRepository(dbContext),
        new MeetingInvitationsRepository(dbContext));

      await handler.Handle(request);
    }
  }
}

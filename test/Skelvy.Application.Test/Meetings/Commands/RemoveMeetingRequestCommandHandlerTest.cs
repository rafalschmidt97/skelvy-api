using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Application.Meetings.Commands.RemoveMeetingRequest;
using Skelvy.Common.Exceptions;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class RemoveMeetingRequestCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveMeetingRequestCommand(1);
      var dbContext = InitializedDbContext();
      var handler = new RemoveMeetingRequestCommandHandler(
        new MeetingUsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext));

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingMeeting()
    {
      var request = new RemoveMeetingRequestCommand(2);
      var dbContext = InitializedDbContext();
      var handler = new RemoveMeetingRequestCommandHandler(
        new MeetingUsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext));

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new RemoveMeetingRequestCommand(1);
      var dbContext = DbContext();
      var handler = new RemoveMeetingRequestCommandHandler(
        new MeetingUsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

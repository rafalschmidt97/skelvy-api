using System.Threading.Tasks;
using Skelvy.Application.Meetings.Commands.RemoveMeetingRequest;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class RemoveMeetingRequestCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveMeetingRequestCommand(1, 1);
      var dbContext = InitializedDbContext();
      var handler = new RemoveMeetingRequestCommandHandler(
        new MeetingRequestsRepository(dbContext),
        new UsersRepository(dbContext));

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithoutSearchingRequest()
    {
      var request = new RemoveMeetingRequestCommand(10, 1);
      var dbContext = InitializedDbContext();
      var handler = new RemoveMeetingRequestCommandHandler(
        new MeetingRequestsRepository(dbContext),
        new UsersRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithoutUser()
    {
      var request = new RemoveMeetingRequestCommand(1, 10);
      var dbContext = InitializedDbContext();
      var handler = new RemoveMeetingRequestCommandHandler(
        new MeetingRequestsRepository(dbContext),
        new UsersRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonUserRequest()
    {
      var request = new RemoveMeetingRequestCommand(1, 2);
      var dbContext = InitializedDbContext();
      var handler = new RemoveMeetingRequestCommandHandler(
        new MeetingRequestsRepository(dbContext),
        new UsersRepository(dbContext));

      await Assert.ThrowsAsync<ForbiddenException>(() =>
        handler.Handle(request));
    }
  }
}

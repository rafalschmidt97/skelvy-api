using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Meetings.Commands.RemoveExpiredMeetings;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class RemoveExpiredMeetingsCommandHandlerTest : DatabaseRequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public RemoveExpiredMeetingsCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveExpiredMeetingsCommand();
      var dbContext = InitializedDbContext();
      var handler = new RemoveExpiredMeetingsCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }
  }
}

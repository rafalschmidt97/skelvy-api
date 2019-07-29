using System;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Meetings.Commands.AddMeetingChatMessage;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class AddMeetingChatMessageCommandHandlerTest : DatabaseRequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public AddMeetingChatMessageCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new AddMeetingChatMessageCommand("Hello World", null, 2);
      var dbContext = InitializedDbContext();
      var handler = new AddMeetingChatMessageCommandHandler(
        new MeetingUsersRepository(dbContext),
        new MeetingChatMessagesRepository(dbContext),
        new AttachmentsRepository(dbContext),
        _mediator.Object,
        Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<MeetingChatMessageDto>(result);
      Assert.NotEqual(default(DateTimeOffset), result.Date);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new AddMeetingChatMessageCommand("Hello World", null, 2);
      var dbContext = DbContext();
      var handler = new AddMeetingChatMessageCommandHandler(
        new MeetingUsersRepository(dbContext),
        new MeetingChatMessagesRepository(dbContext),
        new AttachmentsRepository(dbContext),
        _mediator.Object,
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

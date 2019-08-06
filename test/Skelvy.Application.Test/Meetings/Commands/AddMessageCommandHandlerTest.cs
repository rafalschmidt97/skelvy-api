using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Meetings.Commands.AddMessage;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class AddMessageCommandHandlerTest : DatabaseRequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public AddMessageCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldNotThrowExceptionWithResponse()
    {
      var request = new AddMessageCommand(MessageTypes.Response, "Hello World", null, null, 2, 1);
      var dbContext = InitializedDbContext();
      var handler = new AddMessageCommandHandler(
        new GroupUsersRepository(dbContext),
        new MessagesRepository(dbContext),
        new AttachmentsRepository(dbContext),
        _mediator.Object,
        Mapper());

      var result = await handler.Handle(request);

      Assert.IsAssignableFrom<IList<MessageDto>>(result);
      Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ShouldNotThrowExceptionWithPersistenceAction()
    {
      var request = new AddMessageCommand(MessageTypes.Action, null, null, MessageActionTypes.Seen, 2, 1);
      var dbContext = InitializedDbContext();
      var handler = new AddMessageCommandHandler(
        new GroupUsersRepository(dbContext),
        new MessagesRepository(dbContext),
        new AttachmentsRepository(dbContext),
        _mediator.Object,
        Mapper());

      var result = await handler.Handle(request);

      Assert.IsAssignableFrom<IList<MessageDto>>(result);
      Assert.Equal(1, result.Count);
    }

    [Fact]
    public async Task ShouldNotThrowExceptionWithNonPersistenceAction()
    {
      var request = new AddMessageCommand(MessageTypes.Action, null, null, MessageActionTypes.TypingOn, 2, 1);
      var dbContext = InitializedDbContext();
      var handler = new AddMessageCommandHandler(
        new GroupUsersRepository(dbContext),
        new MessagesRepository(dbContext),
        new AttachmentsRepository(dbContext),
        _mediator.Object,
        Mapper());

      var result = await handler.Handle(request);

      Assert.IsAssignableFrom<IList<MessageDto>>(result);
      Assert.Equal(1, result.Count);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new AddMessageCommand(MessageTypes.Response, "Hello World", null, null, 2, 1);
      var dbContext = DbContext();
      var handler = new AddMessageCommandHandler(
        new GroupUsersRepository(dbContext),
        new MessagesRepository(dbContext),
        new AttachmentsRepository(dbContext),
        _mediator.Object,
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

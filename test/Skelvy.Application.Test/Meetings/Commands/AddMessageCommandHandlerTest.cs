using System;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Meetings.Commands.AddMessage;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Common.Exceptions;
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
    public async Task ShouldNotThrowException()
    {
      var request = new AddMessageCommand("Hello World", null, 2);
      var dbContext = InitializedDbContext();
      var handler = new AddMessageCommandHandler(
        new GroupUsersRepository(dbContext),
        new MessagesRepository(dbContext),
        new AttachmentsRepository(dbContext),
        _mediator.Object,
        Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<MessageDto>(result);
      Assert.NotEqual(default(DateTimeOffset), result.Date);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new AddMessageCommand("Hello World", null, 2);
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

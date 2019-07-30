using System;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMessages;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Queries
{
  public class FindMessagesQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnMessages()
    {
      var request = new FindMessagesQuery(1, 2, DateTimeOffset.UtcNow);
      var dbContext = InitializedDbContext();
      var handler = new FindMessagesQueryHandler(
        new GroupUsersRepository(dbContext),
        new MessagesRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<MessageDto>(x));
      Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindMessagesQuery(1, 1, DateTimeOffset.UtcNow);
      var dbContext = DbContext();
      var handler = new FindMessagesQueryHandler(
        new GroupUsersRepository(dbContext),
        new MessagesRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

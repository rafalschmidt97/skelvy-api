using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.Sync;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Enums;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Users.Queries
{
  public class SyncQueryHandlerTest : RequestTestBase
  {
    private readonly Mock<IMeetingMapper> _mapper;

    public SyncQueryHandlerTest()
    {
      _mapper = new Mock<IMeetingMapper>();
    }

    [Fact]
    public async Task ShouldReturnMeeting()
    {
      var request = new SyncQuery(2, LanguageType.EN);
      var dbContext = InitializedDbContext();
      var handler = new SyncQueryHandler(
        new UsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupsRepository(dbContext),
        new FriendInvitationsRepository(dbContext),
        new MeetingInvitationsRepository(dbContext),
        _mapper.Object,
        Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<SyncModel>(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new SyncQuery(1, LanguageType.EN);
      var dbContext = DbContext();
      var handler = new SyncQueryHandler(
        new UsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupsRepository(dbContext),
        new FriendInvitationsRepository(dbContext),
        new MeetingInvitationsRepository(dbContext),
        _mapper.Object,
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindGroups;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Enums.Users;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Queries
{
  public class FindGroupsQueryHandlerTest : RequestTestBase
  {
    private readonly Mock<IMeetingMapper> _mapper;

    public FindGroupsQueryHandlerTest()
    {
      _mapper = new Mock<IMeetingMapper>();
    }

    [Fact]
    public async Task ShouldReturnMeeting()
    {
      var request = new FindGroupsQuery(2, LanguageType.EN);
      var dbContext = InitializedDbContext();
      var handler = new FindGroupsQueryHandler(
        new UsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupsRepository(dbContext),
        _mapper.Object,
        Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<GroupsModel>(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindGroupsQuery(1, LanguageType.EN);
      var dbContext = DbContext();
      var handler = new FindGroupsQueryHandler(
        new UsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupsRepository(dbContext),
        _mapper.Object,
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

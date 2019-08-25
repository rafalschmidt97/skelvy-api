using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeetings;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Queries
{
  public class FindMeetingsQueryHandlerTest : RequestTestBase
  {
    private readonly Mock<IMeetingMapper> _mapper;

    public FindMeetingsQueryHandlerTest()
    {
      _mapper = new Mock<IMeetingMapper>();
    }

    [Fact]
    public async Task ShouldReturnMeetingsWithGroups()
    {
      var request = new FindMeetingsQuery(1, LanguageType.EN);
      var dbContext = InitializedDbContext();
      _mapper.Setup(x =>
          x.Map(It.IsAny<IList<Meeting>>(), It.IsAny<string>()))
        .ReturnsAsync(new List<MeetingDto>());

      var handler = new FindMeetingsQueryHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupsRepository(dbContext),
        _mapper.Object,
        Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<MeetingModel>(result);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNotExistingUser()
    {
      var request = new FindMeetingsQuery(1, LanguageType.EN);
      var dbContext = DbContext();
      var handler = new FindMeetingsQueryHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupsRepository(dbContext),
        _mapper.Object,
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

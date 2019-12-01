using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeeting;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Queries
{
  public class FindMeetingQueryHandlerTest : RequestTestBase
  {
    private readonly Mock<IMeetingMapper> _mapper;

    public FindMeetingQueryHandlerTest()
    {
      _mapper = new Mock<IMeetingMapper>();
    }

    [Fact]
    public async Task ShouldReturnMeeting()
    {
      var request = new FindMeetingQuery(1, 1, LanguageType.EN);
      var dbContext = InitializedDbContext();
      _mapper.Setup(x =>
          x.Map(It.IsAny<Meeting>(), It.IsAny<string>()))
        .ReturnsAsync(new MeetingDto());

      var handler = new FindMeetingQueryHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        _mapper.Object);

      var result = await handler.Handle(request);

      Assert.IsType<MeetingDto>(result);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNotExistingUser()
    {
      var request = new FindMeetingQuery(1, 1, LanguageType.EN);
      var dbContext = DbContext();
      var handler = new FindMeetingQueryHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        _mapper.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

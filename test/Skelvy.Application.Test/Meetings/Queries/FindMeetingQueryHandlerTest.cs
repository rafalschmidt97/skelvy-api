using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeeting;
using Skelvy.Application.Messages.Queries;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Domain.Enums.Users;
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
      var request = new FindMeetingQuery(2, LanguageType.EN);
      var dbContext = InitializedDbContext();
      _mapper.Setup(x =>
          x.Map(It.IsAny<Meeting>(), It.IsAny<IList<Message>>(), It.IsAny<MeetingRequest>(), It.IsAny<string>()))
        .ReturnsAsync(new MeetingModel(MeetingRequestStatusType.Found, new MeetingDto(), new List<MessageDto>(), new MeetingRequestDto()));

      var handler = new FindMeetingQueryHandler(
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MessagesRepository(dbContext),
        _mapper.Object);

      var result = await handler.Handle(request);

      Assert.IsType<MeetingModel>(result);
      Assert.NotNull(result.Meeting);
    }

    [Fact]
    public async Task ShouldReturnRequest()
    {
      var request = new FindMeetingQuery(1, LanguageType.EN);
      var dbContext = InitializedDbContext();
      _mapper.Setup(x =>
          x.Map(It.IsAny<MeetingRequest>(), It.IsAny<string>()))
        .ReturnsAsync(new MeetingModel(MeetingRequestStatusType.Searching, new MeetingRequestDto()));

      var handler = new FindMeetingQueryHandler(
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MessagesRepository(dbContext),
        _mapper.Object);

      var result = await handler.Handle(request);

      Assert.IsType<MeetingModel>(result);
      Assert.NotNull(result.Request);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindMeetingQuery(1, LanguageType.EN);
      var dbContext = DbContext();
      var handler = new FindMeetingQueryHandler(
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MessagesRepository(dbContext),
        _mapper.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

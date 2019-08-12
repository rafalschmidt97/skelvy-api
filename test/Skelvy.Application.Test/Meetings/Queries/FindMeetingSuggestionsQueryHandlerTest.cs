using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeetingSuggestions;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Queries
{
  public class FindMeetingSuggestionsQueryHandlerTest : RequestTestBase
  {
    private readonly Mock<IMeetingMapper> _mapper;

    public FindMeetingSuggestionsQueryHandlerTest()
    {
      _mapper = new Mock<IMeetingMapper>();
    }

    [Fact]
    public async Task ShouldReturnSuggestions()
    {
      var request = new FindMeetingSuggestionsQuery(4, 1, 1, LanguageType.EN);
      var dbContext = InitializedDbContext();
      _mapper.Setup(x =>
          x.Map(It.IsAny<IList<MeetingRequest>>(), It.IsAny<IList<Meeting>>(), It.IsAny<string>()))
        .ReturnsAsync(new MeetingSuggestionsModel(new List<MeetingRequestWithUserDto>(), new List<MeetingWithUsersDto>()));

      var handler = new FindMeetingSuggestionsQueryHandler(
        new UsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        _mapper.Object);

      var result = await handler.Handle(request);

      Assert.IsType<MeetingSuggestionsModel>(result);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNotExistingUser()
    {
      var request = new FindMeetingSuggestionsQuery(1, 1, 1, LanguageType.EN);
      var dbContext = DbContext();
      var handler = new FindMeetingSuggestionsQueryHandler(
        new UsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        _mapper.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

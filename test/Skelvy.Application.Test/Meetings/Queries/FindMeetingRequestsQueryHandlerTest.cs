using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeetingRequests;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Queries
{
  public class FindMeetingRequestsQueryHandlerTest : RequestTestBase
  {
    private readonly Mock<IMeetingMapper> _mapper;

    public FindMeetingRequestsQueryHandlerTest()
    {
      _mapper = new Mock<IMeetingMapper>();
    }

    [Fact]
    public async Task ShouldReturnRequests()
    {
      var request = new FindMeetingRequestsQuery(1, LanguageType.EN);
      var dbContext = InitializedDbContext();
      _mapper.Setup(x =>
          x.Map(It.IsAny<IList<MeetingRequest>>(), It.IsAny<string>()))
        .ReturnsAsync(new List<MeetingRequestDto>());

      var handler = new FindMeetingRequestsQueryHandler(
        new UsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        _mapper.Object);

      var result = await handler.Handle(request);

      Assert.IsAssignableFrom<IList<MeetingRequestDto>>(result);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNotExistingUser()
    {
      var request = new FindMeetingRequestsQuery(1, LanguageType.EN);
      var dbContext = DbContext();
      var handler = new FindMeetingRequestsQueryHandler(
        new UsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        _mapper.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

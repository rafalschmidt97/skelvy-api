using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeetingInvitations;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Queries
{
  public class FindMeetingInvitationsQueryHandlerTest : RequestTestBase
  {
    private readonly Mock<IMeetingMapper> _mapper;

    public FindMeetingInvitationsQueryHandlerTest()
    {
      _mapper = new Mock<IMeetingMapper>();
    }

    [Fact]
    public async Task ShouldReturnInvitations()
    {
      var request = new FindMeetingInvitationsQuery(1, LanguageType.EN);
      var dbContext = TestDbContextWithMeetingInvitations();
      _mapper.Setup(x =>
          x.Map(It.IsAny<IList<MeetingInvitation>>(), It.IsAny<string>()))
        .ReturnsAsync(new List<SelfMeetingInvitationDto>());

      var handler = new FindMeetingInvitationsQueryHandler(
        new MeetingInvitationsRepository(dbContext),
        new UsersRepository(dbContext),
        _mapper.Object);

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<SelfMeetingInvitationDto>(x));
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindMeetingInvitationsQuery(100, LanguageType.EN);
      var dbContext = TestDbContextWithMeetingInvitations();
      _mapper.Setup(x =>
          x.Map(It.IsAny<IList<MeetingInvitation>>(), It.IsAny<string>()))
        .ReturnsAsync(new List<SelfMeetingInvitationDto>());

      var handler = new FindMeetingInvitationsQueryHandler(
        new MeetingInvitationsRepository(dbContext),
        new UsersRepository(dbContext),
        _mapper.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    private static SkelvyContext TestDbContextWithMeetingInvitations()
    {
      var context = InitializedDbContext();

      var invitation = new MeetingInvitation(2, 1, 1);

      context.MeetingInvitations.AddRange(invitation);
      context.SaveChanges();

      return context;
    }
  }
}

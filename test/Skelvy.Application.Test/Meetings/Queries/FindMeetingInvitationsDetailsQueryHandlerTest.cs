using System.Threading.Tasks;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeetingInvitationsDetails;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Queries
{
  public class FindMeetingInvitationsDetailsQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnInvitations()
    {
      var request = new FindMeetingInvitationsDetailsQuery(1, 2);
      var dbContext = TestDbContextWithMeetingInvitations();

      var handler = new FindMeetingInvitationsDetailsQueryHandler(
        new MeetingInvitationsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<MeetingInvitationDto>(x));
      Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ShouldReturnEmpty()
    {
      var request = new FindMeetingInvitationsDetailsQuery(1, 2);
      var dbContext = InitializedDbContext();

      var handler = new FindMeetingInvitationsDetailsQueryHandler(
        new MeetingInvitationsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<SelfMeetingInvitationDto>(x));
      Assert.Empty(result);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingMeeting()
    {
      var request = new FindMeetingInvitationsDetailsQuery(100, 2);
      var dbContext = TestDbContextWithMeetingInvitations();

      var handler = new FindMeetingInvitationsDetailsQueryHandler(
        new MeetingInvitationsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingGroupUser()
    {
      var request = new FindMeetingInvitationsDetailsQuery(1, 1);
      var dbContext = TestDbContextWithMeetingInvitations();

      var handler = new FindMeetingInvitationsDetailsQueryHandler(
        new MeetingInvitationsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        Mapper());

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

using System.Threading.Tasks;
using Skelvy.Application.Meetings.Queries.FindUsersToInviteToMeeting;
using Skelvy.Application.Users.Queries;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Queries
{
  public class FindUsersToInviteToMeetingQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnInvitations()
    {
      var request = new FindUsersToInviteToMeetingQuery(1, 2, 1);
      var dbContext = TestDbContextWithMeetingInvitations();

      var handler = new FindUsersToInviteToMeetingQueryHandler(
        new MeetingInvitationsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new RelationsRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<UserDto>(x));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingMeeting()
    {
      var request = new FindUsersToInviteToMeetingQuery(100, 2, 1);
      var dbContext = TestDbContextWithMeetingInvitations();

      var handler = new FindUsersToInviteToMeetingQueryHandler(
        new MeetingInvitationsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new RelationsRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingGroupUser()
    {
      var request = new FindUsersToInviteToMeetingQuery(1, 1, 1);
      var dbContext = TestDbContextWithMeetingInvitations();

      var handler = new FindUsersToInviteToMeetingQueryHandler(
        new MeetingInvitationsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new RelationsRepository(dbContext),
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

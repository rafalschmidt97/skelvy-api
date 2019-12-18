using System.Threading.Tasks;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeetingInvitations;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Queries
{
  public class FindMeetingInvitationsQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnInvitations()
    {
      var request = new FindMeetingInvitationsQuery(1);
      var dbContext = TestDbContextWithMeetingInvitations();

      var handler = new FindMeetingInvitationsQueryHandler(
        new MeetingInvitationsRepository(dbContext),
        new UsersRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<SelfMeetingInvitationDto>(x));
      Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ShouldReturnEmpty()
    {
      var request = new FindMeetingInvitationsQuery(2);
      var dbContext = TestDbContextWithMeetingInvitations();

      var handler = new FindMeetingInvitationsQueryHandler(
        new MeetingInvitationsRepository(dbContext),
        new UsersRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<SelfMeetingInvitationDto>(x));
      Assert.Empty(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindMeetingInvitationsQuery(100);
      var dbContext = TestDbContextWithMeetingInvitations();

      var handler = new FindMeetingInvitationsQueryHandler(
        new MeetingInvitationsRepository(dbContext),
        new UsersRepository(dbContext),
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

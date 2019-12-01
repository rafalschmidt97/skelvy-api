using System.Threading.Tasks;
using Skelvy.Application.Groups.Queries.FindGroup;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Enums;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Groups.Queries
{
  public class FindGroupQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnMeetingsWithGroups()
    {
      var request = new FindGroupQuery(2, 1, LanguageType.EN);
      var dbContext = InitializedDbContext();
      var handler = new FindGroupQueryHandler(
        new UsersRepository(dbContext),
        new GroupsRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<GroupDto>(result);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNotExistingUser()
    {
      var request = new FindGroupQuery(2, 1, LanguageType.EN);
      var dbContext = DbContext();
      var handler = new FindGroupQueryHandler(
        new UsersRepository(dbContext),
        new GroupsRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

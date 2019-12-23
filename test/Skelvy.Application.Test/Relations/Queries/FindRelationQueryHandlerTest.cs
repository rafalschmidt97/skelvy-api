using System.Threading.Tasks;
using Skelvy.Application.Relations.Queries;
using Skelvy.Application.Relations.Queries.FindRelation;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Relations.Queries
{
  public class FindRelationQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnRelation()
    {
      var request = new FindRelationQuery(2, 3);
      var dbContext = InitializedDbContext();
      var handler = new FindRelationQueryHandler(
        new UsersRepository(dbContext),
        new RelationsRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<RelationDto>(result);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNotExistingRelation()
    {
      var request = new FindRelationQuery(2, 1);
      var dbContext = InitializedDbContext();
      var handler = new FindRelationQueryHandler(
        new UsersRepository(dbContext),
        new RelationsRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNotExistingUser()
    {
      var request = new FindRelationQuery(100, 3);
      var dbContext = DbContext();
      var handler = new FindRelationQueryHandler(
        new UsersRepository(dbContext),
        new RelationsRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNotExistingRelatedUser()
    {
      var request = new FindRelationQuery(2, 100);
      var dbContext = DbContext();
      var handler = new FindRelationQueryHandler(
        new UsersRepository(dbContext),
        new RelationsRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

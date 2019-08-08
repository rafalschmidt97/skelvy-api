using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;
using Skelvy.Domain.Exceptions;
using Xunit;

namespace Skelvy.Domain.Test.Entities
{
  public class RelationTest
  {
    [Fact]
    public void ShouldBeAborted()
    {
      var entity = new Relation(1, 2, RelationType.Friend);
      entity.Abort();

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.ModifiedAt);
    }

    [Fact]
    public void ShouldThrowExceptionWithAborted()
    {
      var entity = new Relation(1, 2, RelationType.Friend);
      entity.Abort();

      Assert.Throws<DomainException>(() =>
        entity.Abort());
    }
  }
}

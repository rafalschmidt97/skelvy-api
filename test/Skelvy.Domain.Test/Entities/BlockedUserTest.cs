using Skelvy.Domain.Entities;
using Skelvy.Domain.Exceptions;
using Xunit;

namespace Skelvy.Domain.Test.Entities
{
  public class BlockedUserTest
  {
    [Fact]
    public void ShouldBeRemoved()
    {
      var entity = new BlockedUser(1, 2);
      entity.Remove();

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.ModifiedAt);
    }

    [Fact]
    public void ShouldThrowExceptionWithRemoved()
    {
      var entity = new BlockedUser(1, 2);
      entity.Remove();

      Assert.Throws<DomainException>(() =>
        entity.Remove());
    }
  }
}

using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Domain.Exceptions;
using Xunit;

namespace Skelvy.Domain.Test.Entities
{
  public class GroupUserTest
  {
    [Fact]
    public void ShouldBeLeft()
    {
      var entity = new GroupUser(1, 1, 1);
      entity.Leave();

      Assert.True(entity.IsRemoved);
      Assert.Equal(entity.RemovedReason, GroupUserRemovedReasonType.Left);
      Assert.NotNull(entity.ModifiedAt);
    }

    [Fact]
    public void ShouldThrowExceptionWithLeft()
    {
      var entity = new GroupUser(1, 1, 1);
      entity.Leave();

      Assert.Throws<DomainException>(() =>
        entity.Leave());
    }

    [Fact]
    public void ShouldBeAborted()
    {
      var entity = new GroupUser(1, 1, 1);
      entity.Abort();

      Assert.True(entity.IsRemoved);
      Assert.Equal(entity.RemovedReason, GroupUserRemovedReasonType.Aborted);
      Assert.NotNull(entity.ModifiedAt);
    }

    [Fact]
    public void ShouldThrowExceptionWithAborted()
    {
      var entity = new GroupUser(1, 1, 1);
      entity.Abort();

      Assert.Throws<DomainException>(() =>
        entity.Abort());
    }
  }
}

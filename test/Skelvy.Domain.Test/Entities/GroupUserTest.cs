using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;
using Skelvy.Domain.Exceptions;
using Xunit;

namespace Skelvy.Domain.Test.Entities
{
  public class GroupUserTest
  {
    [Fact]
    public void ShouldUpdateRole()
    {
      var entity = new GroupUser(1, 1, 1);
      entity.UpdateRole(GroupUserRoleType.Admin);

      Assert.Equal(entity.Role, GroupUserRoleType.Admin);
      Assert.NotNull(entity.ModifiedAt);
    }

    [Fact]
    public void ShouldThrowExceptionWithUpdateRole()
    {
      var entity = new GroupUser(1, 1, 1);

      Assert.Throws<DomainException>(() =>
          entity.UpdateRole("Example"));
    }

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

    [Fact]
    public void ShouldBeRemoved()
    {
      var entity = new GroupUser(1, 1, 1);
      entity.Remove();

      Assert.True(entity.IsRemoved);
      Assert.Equal(entity.RemovedReason, GroupUserRemovedReasonType.Removed);
      Assert.NotNull(entity.ModifiedAt);
    }

    [Fact]
    public void ShouldThrowExceptionWithRemoved()
    {
      var entity = new GroupUser(1, 1, 1);
      entity.Remove();

      Assert.Throws<DomainException>(() =>
        entity.Remove());
    }
  }
}

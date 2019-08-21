using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;
using Skelvy.Domain.Exceptions;
using Xunit;

namespace Skelvy.Domain.Test.Entities
{
  public class FriendInvitationTest
  {
    [Fact]
    public void ShouldBeAccepted()
    {
      var entity = new FriendInvitation(1, 2);
      entity.Accept();

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.ModifiedAt);
      Assert.Equal(entity.Status, FriendInvitationStatusType.Accepted);
    }

    [Fact]
    public void ShouldThrowExceptionWithAccepted()
    {
      var entity = new FriendInvitation(1, 2);
      entity.Accept();

      Assert.Throws<DomainException>(() =>
        entity.Accept());
    }

    [Fact]
    public void ShouldBeDenied()
    {
      var entity = new FriendInvitation(1, 2);
      entity.Deny();

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.ModifiedAt);
      Assert.Equal(entity.Status, FriendInvitationStatusType.Denied);
    }

    [Fact]
    public void ShouldThrowExceptionWithDenied()
    {
      var entity = new FriendInvitation(1, 2);
      entity.Deny();

      Assert.Throws<DomainException>(() =>
        entity.Deny());
    }
  }
}

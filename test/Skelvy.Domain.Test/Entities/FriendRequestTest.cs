using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;
using Skelvy.Domain.Exceptions;
using Xunit;

namespace Skelvy.Domain.Test.Entities
{
  public class FriendRequestTest
  {
    [Fact]
    public void ShouldBeAccepted()
    {
      var entity = new FriendRequest(1, 2);
      entity.Accept();

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.ModifiedAt);
      Assert.Equal(entity.Status, FriendRequestStatusType.Accepted);
    }

    [Fact]
    public void ShouldThrowExceptionWithAccepted()
    {
      var entity = new FriendRequest(1, 2);
      entity.Accept();

      Assert.Throws<DomainException>(() =>
        entity.Accept());
    }

    [Fact]
    public void ShouldBeDenied()
    {
      var entity = new FriendRequest(1, 2);
      entity.Deny();

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.ModifiedAt);
      Assert.Equal(entity.Status, FriendRequestStatusType.Denied);
    }

    [Fact]
    public void ShouldThrowExceptionWithDenied()
    {
      var entity = new FriendRequest(1, 2);
      entity.Deny();

      Assert.Throws<DomainException>(() =>
        entity.Deny());
    }
  }
}

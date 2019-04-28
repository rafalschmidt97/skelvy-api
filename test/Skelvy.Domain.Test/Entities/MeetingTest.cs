using System;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Domain.Exceptions;
using Xunit;

namespace Skelvy.Domain.Test.Entities
{
  public class MeetingTest
  {
    [Fact]
    public void ShouldBeAborted()
    {
      var entity = new Meeting(DateTimeOffset.UtcNow, 1, 1, 1);
      entity.Abort();

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.ModifiedAt);
      Assert.Equal(entity.RemovedReason, MeetingRemovedReasonTypes.Aborted);
    }

    [Fact]
    public void ShouldThrowExceptionWithAborted()
    {
      var entity = new Meeting(DateTimeOffset.UtcNow, 1, 1, 1);
      entity.Abort();

      Assert.Throws<DomainException>(() =>
        entity.Abort());
    }

    [Fact]
    public void ShouldBeExpired()
    {
      var entity = new Meeting(DateTimeOffset.UtcNow, 1, 1, 1);
      entity.Expire();

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.ModifiedAt);
      Assert.Equal(entity.RemovedReason, MeetingRemovedReasonTypes.Expired);
    }

    [Fact]
    public void ShouldThrowExceptionWithExpired()
    {
      var entity = new Meeting(DateTimeOffset.UtcNow, 1, 1, 1);
      entity.Expire();

      Assert.Throws<DomainException>(() =>
        entity.Expire());
    }
  }
}

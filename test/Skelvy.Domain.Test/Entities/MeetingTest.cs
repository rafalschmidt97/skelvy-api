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
    public void ShouldBeUpdated()
    {
      var entity = new Meeting(DateTimeOffset.UtcNow, 1, 1, 4, false, false, 1, 1);
      entity.Update(DateTimeOffset.UtcNow.AddDays(1), 2, 2, 6, true, true);

      Assert.True(entity.IsPrivate);
      Assert.NotNull(entity.ModifiedAt);
    }

    [Fact]
    public void ShouldThrowExceptionWhileUpdatingWithInvalidDate()
    {
      var entity = new Meeting(DateTimeOffset.UtcNow, 1, 1, 4, false, false, 1, 1);

      Assert.Throws<DomainException>(() =>
        entity.Update(DateTimeOffset.UtcNow.AddDays(-1), 2, 2, 6, true, true));
    }

    [Fact]
    public void ShouldBeAborted()
    {
      var entity = new Meeting(DateTimeOffset.UtcNow, 1, 1, 4, false, false, 1, 1);
      entity.Abort();

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.ModifiedAt);
      Assert.Equal(entity.RemovedReason, MeetingRemovedReasonType.Aborted);
    }

    [Fact]
    public void ShouldThrowExceptionWithAborted()
    {
      var entity = new Meeting(DateTimeOffset.UtcNow, 1, 1, 4, false, false, 1, 1);
      entity.Abort();

      Assert.Throws<DomainException>(() =>
        entity.Abort());
    }

    [Fact]
    public void ShouldBeExpired()
    {
      var entity = new Meeting(DateTimeOffset.UtcNow, 1, 1, 4, false, false, 1, 1);
      entity.Expire();

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.ModifiedAt);
      Assert.Equal(entity.RemovedReason, MeetingRemovedReasonType.Expired);
    }

    [Fact]
    public void ShouldThrowExceptionWithExpired()
    {
      var entity = new Meeting(DateTimeOffset.UtcNow, 1, 1, 4, false, false, 1, 1);
      entity.Expire();

      Assert.Throws<DomainException>(() =>
        entity.Expire());
    }
  }
}

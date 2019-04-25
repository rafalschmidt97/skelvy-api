using System;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;
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
      Assert.NotNull(entity.RemovedAt);
      Assert.Equal(entity.RemovedReason, MeetingRemovedReasonTypes.Aborted);
    }

    [Fact]
    public void ShouldBeExpired()
    {
      var entity = new Meeting(DateTimeOffset.UtcNow, 1, 1, 1);
      entity.Expire();

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.RemovedAt);
      Assert.Equal(entity.RemovedReason, MeetingRemovedReasonTypes.Expired);
    }
  }
}

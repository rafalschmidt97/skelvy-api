using System;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;
using Xunit;

namespace Skelvy.Domain.Test
{
  public class MeetingRequestTest
  {
    [Fact]
    public void ShouldBeAborted()
    {
      var entity = new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1);
      entity.Abort();

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.RemovedAt);
      Assert.Equal(entity.RemovedReason, MeetingRequestRemovedReasonTypes.Aborted);
    }

    [Fact]
    public void ShouldBeExpired()
    {
      var entity = new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1);
      entity.Expire();

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.RemovedAt);
      Assert.Equal(entity.RemovedReason, MeetingRequestRemovedReasonTypes.Expired);
    }

    [Fact]
    public void ShouldBeMarkedAsFound()
    {
      var entity = new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1);
      entity.MarkAsFound();

      Assert.True(entity.IsFound);
      Assert.False(entity.IsSearching);
    }

    [Fact]
    public void ShouldBeMarkedAsSearching()
    {
      var entity = new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1);

      Assert.True(entity.IsSearching);
      Assert.False(entity.IsFound);
    }
  }
}

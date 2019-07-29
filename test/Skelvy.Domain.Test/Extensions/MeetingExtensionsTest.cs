using System;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Extensions;
using Xunit;

namespace Skelvy.Domain.Test.Extensions
{
  public class MeetingExtensionsTest
  {
    [Fact]
    public void ShouldReturnCalculatedDistance()
    {
      var meeting = new Meeting(DateTimeOffset.UtcNow, 1, 1, 1, 1);
      var request = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), 18, 25, 2, 2, 1);

      var distance = meeting.GetDistance(request);

      Assert.InRange(distance, 156, 158);
      Assert.Equal(157, distance, 0);
    }
  }
}

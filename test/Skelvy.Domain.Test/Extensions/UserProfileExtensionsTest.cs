using System;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;
using Skelvy.Domain.Extensions;
using Xunit;

namespace Skelvy.Domain.Test.Extensions
{
  public class UserProfileExtensionsTest
  {
    [Fact]
    public void ShouldReturnAge()
    {
      var profile = new UserProfile("Example", DateTimeOffset.UtcNow.AddYears(-18), GenderTypes.Male, 1);

      var age = profile.GetAge();

      Assert.Equal(18, age);
    }

    [Fact]
    public void ShouldBeBetweenMeetingRequestAgeRange()
    {
      var profile = new UserProfile("Example", DateTimeOffset.UtcNow.AddYears(-18), GenderTypes.Male, 1);
      var request = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), 18, 25, 2, 2, 1);

      var result = profile.IsWithinMeetingRequestAgeRange(request);

      Assert.True(result);
    }

    [Fact]
    public void ShouldNotBeBetweenMeetingRequestAgeRange()
    {
      var profile = new UserProfile("Example", DateTimeOffset.UtcNow.AddYears(-18), GenderTypes.Male, 1);
      var request = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), 20, 25, 2, 2, 1);

      var result = profile.IsWithinMeetingRequestAgeRange(request);

      Assert.False(result);
    }
  }
}

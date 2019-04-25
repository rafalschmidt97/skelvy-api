using System;
using Skelvy.Domain.Entities;

namespace Skelvy.Domain.Extensions
{
  public static class UserProfileExtension
  {
    public static int GetAge(this UserProfile profile)
    {
      var age = DateTimeOffset.UtcNow.Year - profile.Birthday.Year;
      if (DateTimeOffset.UtcNow.DayOfYear < profile.Birthday.DayOfYear)
      {
        age = age - 1;
      }

      return age;
    }

    public static bool IsWithinMeetingRequestAgeRange(this UserProfile profile, MeetingRequest request)
    {
      var age = GetAge(profile);
      return age >= request.MinAge && (request.MaxAge >= 55 || age <= request.MaxAge);
    }
  }
}

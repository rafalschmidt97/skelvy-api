using Skelvy.Common.Extensions;
using Skelvy.Domain.Entities;

namespace Skelvy.Domain.Extensions
{
  public static class UserProfileExtension
  {
    public static int GetAge(this UserProfile profile)
    {
      return profile.Birthday.GetAge();
    }

    public static bool IsWithinMeetingRequestAgeRange(this UserProfile profile, MeetingRequest request)
    {
      var age = GetAge(profile);
      return age >= request.MinAge && (request.MaxAge >= 55 || age <= request.MaxAge);
    }
  }
}

using Skelvy.Common.Extensions;
using Skelvy.Domain.Entities;

namespace Skelvy.Domain.Extensions
{
  public static class ProfileExtension
  {
    public static int GetAge(this Profile profile)
    {
      return profile.Birthday.GetAge();
    }

    public static bool IsWithinMeetingRequestAgeRange(this Profile profile, MeetingRequest request)
    {
      var age = GetAge(profile);
      return age >= request.MinAge && (request.MaxAge >= 55 || age <= request.MaxAge);
    }
  }
}

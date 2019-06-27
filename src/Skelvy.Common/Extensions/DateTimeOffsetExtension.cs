using System;

namespace Skelvy.Common.Extensions
{
  public static class DateTimeOffsetExtension
  {
    public static int GetAge(this DateTimeOffset date)
    {
      var age = DateTimeOffset.UtcNow.Year - date.Year;
      if (DateTimeOffset.UtcNow.DayOfYear < date.DayOfYear)
      {
        age = age - 1;
      }

      return age;
    }
  }
}

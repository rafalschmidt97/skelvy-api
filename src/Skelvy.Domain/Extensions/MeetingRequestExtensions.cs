using System;
using System.Collections.Generic;
using System.Linq;
using Skelvy.Domain.Entities;

namespace Skelvy.Domain.Extensions
{
  public static class MeetingRequestExtensions
  {
    public static IEnumerable<DateTimeOffset> FindCommonDates(this MeetingRequest request1, MeetingRequest request2)
    {
      var dates = new List<DateTimeOffset>();

      for (var i = request1.MinDate; i <= request1.MaxDate; i = i.AddDays(1))
      {
        dates.Add(i);
      }

      var commonDates = new List<DateTimeOffset>();

      foreach (var date in dates)
      {
        if (date >= request1.MinDate && date < request1.MaxDate &&
            date >= request2.MinDate && date < request2.MaxDate)
        {
          commonDates.Add(date);
        }
      }

      return dates;
    }

    public static DateTimeOffset FindCommonDate(this MeetingRequest request1, MeetingRequest request2)
    {
      return FindCommonDates(request1, request2).FirstOrDefault();
    }

    public static IEnumerable<int> FindCommonDrinksId(this MeetingRequest request1, MeetingRequest request2)
    {
      return request1.Drinks
        .Where(x => request2.Drinks.Any(y => y.DrinkId == x.DrinkId))
        .Select(x => x.DrinkId)
        .ToList();
    }

    public static int FindCommonDrinkId(this MeetingRequest request1, MeetingRequest request2)
    {
      return FindCommonDrinksId(request1, request2).FirstOrDefault();
    }
  }
}

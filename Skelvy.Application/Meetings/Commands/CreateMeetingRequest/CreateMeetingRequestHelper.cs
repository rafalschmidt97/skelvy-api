using System;
using System.Collections.Generic;
using System.Linq;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Commands.CreateMeetingRequest
{
  public static class CreateMeetingRequestHelper
  {
    public static double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
    {
      var d1 = latitude1 * (Math.PI / 180.0);
      var num1 = longitude1 * (Math.PI / 180.0);
      var d2 = latitude2 * (Math.PI / 180.0);
      var num2 = longitude2 * (Math.PI / 180.0) - num1;
      var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
               Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

      return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))) / 1000;
    }

    public static int CalculateAge(DateTime date)
    {
      var age = DateTime.Now.Year - date.Year;
      if (DateTime.Now.DayOfYear < date.DayOfYear)
      {
        age = age - 1;
      }

      return age;
    }

    public static DateTime FindCommonDate(MeetingRequest request1, MeetingRequest request2)
    {
      var dates = new List<DateTime>();

      for (var i = request1.MinDate; i <= request1.MaxDate; i = i.AddDays(1))
      {
        dates.Add(i);
      }

      var commonDates = new List<DateTime>();

      foreach (var date in dates)
      {
        if (date >= request1.MinDate && date < request1.MaxDate &&
            date >= request2.MinDate && date < request2.MaxDate)
        {
          commonDates.Add(date);
        }
      }

      return dates.First();
    }

    public static int FindCommonDrink(MeetingRequest request1, MeetingRequest request2)
    {
      return request1.Drinks.First(x => request2.Drinks.Any(y => y.DrinkId == x.DrinkId)).DrinkId;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Exceptions;

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
        if (date >= request2.MinDate && date <= request2.MaxDate)
        {
          commonDates.Add(date);
        }
      }

      return commonDates;
    }

    public static DateTimeOffset FindCommonDate(this MeetingRequest request1, MeetingRequest request2)
    {
      return FindCommonDates(request1, request2).FirstOrDefault();
    }

    public static DateTimeOffset FindRequiredCommonDate(this MeetingRequest request1, MeetingRequest request2)
    {
      var date = FindCommonDate(request1, request2);
      return date != default(DateTimeOffset) ? date : throw new DomainException($"{nameof(MeetingRequest)}[Id = {request1.Id}, Id = {request2.Id}]) don't have common date.");
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

    public static int FindRequiredCommonDrinkId(this MeetingRequest request1, MeetingRequest request2)
    {
      var drink = FindCommonDrinkId(request1, request2);
      return drink != default(int) ? drink : throw new DomainException($"{nameof(MeetingRequest)}[Id = {request1.Id}, Id = {request2.Id}]) don't have common drink.");
    }
  }
}

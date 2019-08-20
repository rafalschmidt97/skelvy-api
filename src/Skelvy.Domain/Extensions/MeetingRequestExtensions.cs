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
      return date != default ? date : throw new DomainException($"{nameof(MeetingRequest)}({request1.Id}, {request2.Id}) do not have common date.");
    }

    public static IEnumerable<int> FindCommonActivitiesId(this MeetingRequest request1, MeetingRequest request2)
    {
      return request1.Activities
        .Where(x => request2.Activities.Any(y => y.ActivityId == x.ActivityId))
        .Select(x => x.ActivityId)
        .ToList();
    }

    public static int FindCommonActivityId(this MeetingRequest request1, MeetingRequest request2)
    {
      return FindCommonActivitiesId(request1, request2).FirstOrDefault();
    }

    public static int FindRequiredCommonActivityId(this MeetingRequest request1, MeetingRequest request2)
    {
      var activity = FindCommonActivityId(request1, request2);
      return activity != default ? activity : throw new DomainException($"{nameof(MeetingRequest)}({request1.Id}, {request2.Id}) don't have common activity.");
    }
  }
}

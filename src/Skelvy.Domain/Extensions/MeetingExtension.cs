using System;
using Skelvy.Domain.Entities;

namespace Skelvy.Domain.Extensions
{
  public static class MeetingExtension
  {
    public static double GetDistance(this Meeting meeting, MeetingRequest request)
    {
      return GetDistance(meeting.Latitude, meeting.Longitude, request.Latitude, request.Longitude);
    }

    public static double GetDistance(this MeetingRequest request1, MeetingRequest request2)
    {
      return GetDistance(request1.Latitude, request1.Longitude, request2.Latitude, request2.Longitude);
    }

    public static double GetDistance(this MeetingRequest request, Meeting meeting)
    {
      return GetDistance(request.Latitude, request.Longitude, meeting.Latitude, meeting.Longitude);
    }

    private static double GetDistance(double latitude1, double longitude1, double latitude2, double longitude2)
    {
      var d1 = latitude1 * (Math.PI / 180.0);
      var num1 = longitude1 * (Math.PI / 180.0);
      var d2 = latitude2 * (Math.PI / 180.0);
      var num2 = longitude2 * (Math.PI / 180.0) - num1;
      var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
               Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

      return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))) / 1000;
    }
  }
}

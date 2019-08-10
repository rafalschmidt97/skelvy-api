using System;
using System.Collections.Generic;
using System.Linq;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Exceptions;
using Skelvy.Domain.Extensions;
using Xunit;

namespace Skelvy.Domain.Test.Extensions
{
  public class MeetingRequestExtensionsTest
  {
    [Fact]
    public void ShouldReturnCommonDates()
    {
      var request1 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), 18, 25, 2, 2, 1);
      var request2 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddDays(2), 18, 25, 2, 2, 1);

      var dates = request1.FindCommonDates(request2).ToList();

      Assert.Equal(3, dates.Count);
      Assert.True(dates[0] >= DateTimeOffset.UtcNow.AddDays(-1).AddMinutes(-1));
      Assert.True(dates[0] <= DateTimeOffset.UtcNow.AddDays(1).AddMinutes(1));
    }

    [Fact]
    public void ShouldReturnEmptyCommonDates()
    {
      var request1 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-3), DateTimeOffset.UtcNow.AddDays(-1), 18, 25, 2, 2, 1);
      var request2 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow.AddDays(3), 18, 25, 2, 2, 1);

      var dates = request1.FindCommonDates(request2).ToList();

      Assert.Empty(dates);
    }

    [Fact]
    public void ShouldReturnCommonDate()
    {
      var request1 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), 18, 25, 2, 2, 1);
      var request2 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddDays(2), 18, 25, 2, 2, 1);

      var date = request1.FindCommonDate(request2);

      Assert.NotEqual(default, date);
      Assert.True(date >= DateTimeOffset.UtcNow.AddDays(-1).AddMinutes(-1));
      Assert.True(date <= DateTimeOffset.UtcNow.AddDays(1).AddMinutes(1));
    }

    [Fact]
    public void ShouldReturnDefaultCommonDate()
    {
      var request1 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-3), DateTimeOffset.UtcNow.AddDays(-1), 18, 25, 2, 2, 1);
      var request2 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow.AddDays(3), 18, 25, 2, 2, 1);

      var date = request1.FindCommonDate(request2);

      Assert.Equal(default, date);
    }

    [Fact]
    public void ShouldReturnCommonDateWithOneSame()
    {
      var min = DateTimeOffset.UtcNow.AddDays(1);
      var request1 = new MeetingRequest(min, min, 18, 25, 2, 2, 1);
      var request2 = new MeetingRequest(min, min, 18, 25, 2, 2, 1);

      var date = request1.FindCommonDate(request2);

      Assert.NotEqual(default, date);
      Assert.Equal(min, date);
    }

    [Fact]
    public void ShouldCommonDateThrowException()
    {
      var request1 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-3), DateTimeOffset.UtcNow.AddDays(-1), 18, 25, 2, 2, 1);
      var request2 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow.AddDays(3), 18, 25, 2, 2, 1);

      Assert.Throws<DomainException>(() =>
        request1.FindRequiredCommonDate(request2));
    }

    [Fact]
    public void ShouldReturnCommonActivitiesId()
    {
      var request1 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), 18, 25, 2, 2, 1);
      request1.Activities = new List<MeetingRequestActivity>
      {
        new MeetingRequestActivity(1, 1),
        new MeetingRequestActivity(1, 2),
      };
      var request2 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddDays(2), 18, 25, 2, 2, 1);
      request2.Activities = new List<MeetingRequestActivity>
      {
        new MeetingRequestActivity(2, 2),
        new MeetingRequestActivity(2, 3),
      };

      var activitiesId = request1.FindCommonActivitiesId(request2).ToList();

      Assert.Single(activitiesId);
      Assert.Equal(2, activitiesId[0]);
    }

    [Fact]
    public void ShouldReturnEmptyCommonActivitiesId()
    {
      var request1 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), 18, 25, 2, 2, 1);
      request1.Activities = new List<MeetingRequestActivity>
      {
        new MeetingRequestActivity(1, 1),
        new MeetingRequestActivity(1, 2),
      };
      var request2 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddDays(2), 18, 25, 2, 2, 1);
      request2.Activities = new List<MeetingRequestActivity>
      {
        new MeetingRequestActivity(2, 3),
        new MeetingRequestActivity(2, 4),
      };

      var activitiesId = request1.FindCommonActivitiesId(request2).ToList();

      Assert.Empty(activitiesId);
    }

    [Fact]
    public void ShouldReturnCommonActivityId()
    {
      var request1 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), 18, 25, 2, 2, 1);
      request1.Activities = new List<MeetingRequestActivity>
      {
        new MeetingRequestActivity(1, 1),
        new MeetingRequestActivity(1, 2),
      };
      var request2 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddDays(2), 18, 25, 2, 2, 1);
      request2.Activities = new List<MeetingRequestActivity>
      {
        new MeetingRequestActivity(2, 2),
        new MeetingRequestActivity(2, 3),
      };

      var drinkTypeId = request1.FindCommonActivityId(request2);

      Assert.Equal(2, drinkTypeId);
    }

    [Fact]
    public void ShouldReturnDefaultCommonActivityId()
    {
      var request1 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), 18, 25, 2, 2, 1);
      request1.Activities = new List<MeetingRequestActivity>
      {
        new MeetingRequestActivity(1, 1),
        new MeetingRequestActivity(1, 2),
      };
      var request2 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddDays(2), 18, 25, 2, 2, 1);
      request2.Activities = new List<MeetingRequestActivity>
      {
        new MeetingRequestActivity(2, 3),
        new MeetingRequestActivity(2, 4),
      };

      var drinkTypeId = request1.FindCommonActivityId(request2);

      Assert.Equal(default, drinkTypeId);
    }

    [Fact]
    public void ShouldCommonActivityThrowException()
    {
      var request1 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), 18, 25, 2, 2, 1);
      request1.Activities = new List<MeetingRequestActivity>
      {
        new MeetingRequestActivity(1, 1),
        new MeetingRequestActivity(1, 2),
      };
      var request2 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddDays(2), 18, 25, 2, 2, 1);
      request2.Activities = new List<MeetingRequestActivity>
      {
        new MeetingRequestActivity(2, 3),
        new MeetingRequestActivity(2, 4),
      };

      Assert.Throws<DomainException>(() =>
        request1.FindRequiredCommonActivityId(request2));
    }
  }
}

using System;
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

      Assert.NotEqual(default(DateTimeOffset), date);
      Assert.True(date >= DateTimeOffset.UtcNow.AddDays(-1).AddMinutes(-1));
      Assert.True(date <= DateTimeOffset.UtcNow.AddDays(1).AddMinutes(1));
    }

    [Fact]
    public void ShouldReturnDefaultCommonDate()
    {
      var request1 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-3), DateTimeOffset.UtcNow.AddDays(-1), 18, 25, 2, 2, 1);
      var request2 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow.AddDays(3), 18, 25, 2, 2, 1);

      var date = request1.FindCommonDate(request2);

      Assert.Equal(default(DateTimeOffset), date);
    }

    [Fact]
    public void ShouldReturnCommonDateWithOneSame()
    {
      var min = DateTimeOffset.UtcNow.AddDays(1);
      var request1 = new MeetingRequest(min, min, 18, 25, 2, 2, 1);
      var request2 = new MeetingRequest(min, min, 18, 25, 2, 2, 1);

      var date = request1.FindCommonDate(request2);

      Assert.NotEqual(default(DateTimeOffset), date);
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
    public void ShouldReturnCommonDrinksId()
    {
      var request1 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), 18, 25, 2, 2, 1);
      request1.Drinks.Add(new MeetingRequestDrink(1, 1));
      request1.Drinks.Add(new MeetingRequestDrink(1, 2));
      var request2 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddDays(2), 18, 25, 2, 2, 1);
      request2.Drinks.Add(new MeetingRequestDrink(2, 2));
      request2.Drinks.Add(new MeetingRequestDrink(2, 3));

      var drinksId = request1.FindCommonDrinksId(request2).ToList();

      Assert.Single(drinksId);
      Assert.Equal(2, drinksId[0]);
    }

    [Fact]
    public void ShouldReturnEmptyCommonDrinksId()
    {
      var request1 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), 18, 25, 2, 2, 1);
      request1.Drinks.Add(new MeetingRequestDrink(1, 1));
      request1.Drinks.Add(new MeetingRequestDrink(1, 2));
      var request2 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddDays(2), 18, 25, 2, 2, 1);
      request2.Drinks.Add(new MeetingRequestDrink(2, 3));
      request2.Drinks.Add(new MeetingRequestDrink(2, 4));

      var drinksId = request1.FindCommonDrinksId(request2).ToList();

      Assert.Empty(drinksId);
    }

    [Fact]
    public void ShouldReturnCommonDrinkId()
    {
      var request1 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), 18, 25, 2, 2, 1);
      request1.Drinks.Add(new MeetingRequestDrink(1, 1));
      request1.Drinks.Add(new MeetingRequestDrink(1, 2));
      var request2 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddDays(2), 18, 25, 2, 2, 1);
      request2.Drinks.Add(new MeetingRequestDrink(2, 2));
      request2.Drinks.Add(new MeetingRequestDrink(2, 3));

      var drinkId = request1.FindCommonDrinkId(request2);

      Assert.Equal(2, drinkId);
    }

    [Fact]
    public void ShouldReturnDefaultCommonDrinkId()
    {
      var request1 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), 18, 25, 2, 2, 1);
      request1.Drinks.Add(new MeetingRequestDrink(1, 1));
      request1.Drinks.Add(new MeetingRequestDrink(1, 2));
      var request2 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddDays(2), 18, 25, 2, 2, 1);
      request2.Drinks.Add(new MeetingRequestDrink(2, 3));
      request2.Drinks.Add(new MeetingRequestDrink(2, 4));

      var drinkId = request1.FindCommonDrinkId(request2);

      Assert.Equal(default(int), drinkId);
    }

    [Fact]
    public void ShouldCommonDrinkThrowException()
    {
      var request1 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), 18, 25, 2, 2, 1);
      request1.Drinks.Add(new MeetingRequestDrink(1, 1));
      request1.Drinks.Add(new MeetingRequestDrink(1, 2));
      var request2 = new MeetingRequest(DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddDays(2), 18, 25, 2, 2, 1);
      request2.Drinks.Add(new MeetingRequestDrink(2, 3));
      request2.Drinks.Add(new MeetingRequestDrink(2, 4));

      Assert.Throws<DomainException>(() =>
        request1.FindRequiredCommonDrinkId(request2));
    }
  }
}

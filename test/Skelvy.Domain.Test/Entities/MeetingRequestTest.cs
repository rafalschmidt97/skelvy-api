using System;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;
using Skelvy.Domain.Exceptions;
using Xunit;

namespace Skelvy.Domain.Test.Entities
{
  public class MeetingRequestTest
  {
    [Fact]
    public void ShouldBeUpdatedWithDescription()
    {
      var entity = new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1);
      entity.Update(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, " Description ");

      Assert.Equal("Description", entity.Description);
      Assert.NotNull(entity.ModifiedAt);
    }

    [Fact]
    public void ShouldThrowExceptionWithUpdatingInvalidData()
    {
      var entity = new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1);

      Assert.Throws<DomainException>(() =>
        entity.Update(DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow, 25, 18, 1, 1, "Description"));
    }

    [Fact]
    public void ShouldBeAborted()
    {
      var entity = new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1);
      entity.Abort();

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.ModifiedAt);
      Assert.Equal(entity.RemovedReason, MeetingRequestRemovedReasonType.Aborted);
    }

    [Fact]
    public void ShouldThrowExceptionWithAborted()
    {
      var entity = new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1);
      entity.Abort();

      Assert.Throws<DomainException>(() =>
        entity.Abort());
    }

    [Fact]
    public void ShouldBeExpired()
    {
      var entity = new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1);
      entity.Expire();

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.ModifiedAt);
      Assert.Equal(entity.RemovedReason, MeetingRequestRemovedReasonType.Expired);
    }

    [Fact]
    public void ShouldThrowExceptionWithExpired()
    {
      var entity = new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1);
      entity.Expire();

      Assert.Throws<DomainException>(() =>
        entity.Expire());
    }

    [Fact]
    public void ShouldBeMarkedAsFound()
    {
      var entity = new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1);
      entity.MarkAsFound();

      Assert.True(entity.IsFound);
      Assert.False(entity.IsSearching);
    }

    [Fact]
    public void ShouldThrowExceptionWithMarkedAsFound()
    {
      var entity = new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1);
      entity.MarkAsFound();

      Assert.Throws<DomainException>(() =>
        entity.MarkAsFound());
    }

    [Fact]
    public void ShouldThrowExceptionWithMarkedAsFoundWhenRemoved()
    {
      var entity = new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1);
      entity.Expire();

      Assert.Throws<DomainException>(() =>
        entity.MarkAsFound());
    }

    [Fact]
    public void ShouldBeMarkedAsSearching()
    {
      var entity = new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1);

      Assert.True(entity.IsSearching);
      Assert.False(entity.IsFound);
    }

    [Fact]
    public void ShouldThrowExceptionWithMarkedAsSearching()
    {
      var entity = new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1);

      Assert.Throws<DomainException>(() =>
        entity.MarkAsSearching());
    }

    [Fact]
    public void ShouldThrowExceptionWithMarkedAsSearchingWhenRemoved()
    {
      var entity = new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1);
      entity.MarkAsFound();
      entity.Expire();

      Assert.Throws<DomainException>(() =>
        entity.MarkAsSearching());
    }
  }
}

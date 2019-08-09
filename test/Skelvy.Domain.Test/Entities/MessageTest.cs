using System;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Domain.Exceptions;
using Xunit;

namespace Skelvy.Domain.Test.Entities
{
  public class MessageTest
  {
    [Fact]
    public void ShouldUpdateSeen()
    {
      var date = DateTimeOffset.UtcNow.AddHours(-2);
      var entity = new Message(MessageType.Action, date, null, null, MessageActionType.Seen, 1, 1);
      entity.UpdateSeen();

      Assert.NotEqual(entity.Date, date);
    }

    [Fact]
    public void ShouldThrowExceptionWithResponseType()
    {
      var entity = new Message(MessageType.Response, DateTimeOffset.UtcNow, null, null, MessageActionType.Seen, 1, 1);

      Assert.Throws<DomainException>(() =>
        entity.UpdateSeen());
    }

    [Fact]
    public void ShouldThrowExceptionWithNonSeenType()
    {
      var entity = new Message(MessageType.Action, DateTimeOffset.UtcNow, null, null, MessageActionType.TypingOn, 1, 1);

      Assert.Throws<DomainException>(() =>
        entity.UpdateSeen());
    }
  }
}

using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;

namespace Skelvy.Domain.Entities
{
  public class User : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public User(string email, string language)
    {
      Email = email;
      Language = language;

      CreatedAt = DateTimeOffset.UtcNow;
      Roles = new List<UserRole>();
      MeetingChatMessages = new List<MeetingChatMessage>();
      MeetingRequests = new List<MeetingRequest>();
    }

    public User(int id, string email, string language)
      : this(email, language)
    {
      Id = id;
    }

    public int Id { get; private set; }
    public string Email { get; private set; }
    public string Language { get; private set; }
    public string FacebookId { get; private set; }
    public string GoogleId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ModifiedAt { get; private set; }
    public bool IsRemoved { get; private set; }
    public DateTimeOffset? RemovedAt { get; private set; }
    public DateTimeOffset? ForgottenAt { get; private set; }
    public bool IsDisabled { get; private set; }
    public DateTimeOffset? DisabledAt { get; private set; }
    public string DisabledReason { get; private set; }

    public UserProfile Profile { get; private set; }
    public IList<UserRole> Roles { get; private set; }
    public IList<MeetingRequest> MeetingRequests { get; private set; }
    public IList<MeetingChatMessage> MeetingChatMessages { get; private set; }

    public void RegisterFacebook(string facebookId)
    {
      FacebookId = facebookId;
    }

    public void RegisterGoogle(string googleId)
    {
      GoogleId = googleId;
    }

    public void UpdateLanguage(string language)
    {
      Language = language;
      ModifiedAt = DateTimeOffset.UtcNow;
    }

    public void Remove(DateTimeOffset forgottenAt)
    {
      IsRemoved = true;
      RemovedAt = DateTimeOffset.UtcNow;
      ForgottenAt = forgottenAt;
    }

    public void Disable(string reason)
    {
      IsDisabled = true;
      DisabledAt = DateTimeOffset.UtcNow;
      DisabledReason = reason;
    }
  }
}

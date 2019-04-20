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

      CreatedDate = DateTimeOffset.UtcNow;
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
    public DateTimeOffset CreatedDate { get; private set; }
    public DateTimeOffset? ModifiedDate { get; private set; }
    public bool IsRemoved { get; private set; }
    public DateTimeOffset? RemovedDate { get; private set; }
    public DateTimeOffset? ForgottenDate { get; private set; }
    public bool IsDisabled { get; private set; }
    public DateTimeOffset? DisabledDate { get; private set; }
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
      ModifiedDate = DateTimeOffset.UtcNow;
    }

    public void Remove(DateTimeOffset forgottenDate)
    {
      IsRemoved = true;
      RemovedDate = DateTimeOffset.UtcNow;
      ForgottenDate = forgottenDate;
    }

    public void Disable(string reason)
    {
      IsDisabled = true;
      DisabledDate = DateTimeOffset.UtcNow;
      DisabledReason = reason;
    }
  }
}

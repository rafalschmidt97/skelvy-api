using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;

namespace Skelvy.Domain.Entities
{
  public class User : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public User()
    {
      CreatedDate = DateTimeOffset.UtcNow;
      Roles = new List<UserRole>();
      MeetingChatMessages = new List<MeetingChatMessage>();
      MeetingRequests = new List<MeetingRequest>();
    }

    public int Id { get; set; }
    public string Email { get; set; }
    public string Language { get; set; }
    public string FacebookId { get; set; }
    public string GoogleId { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
    public bool IsRemoved { get; set; }
    public DateTimeOffset? RemovedDate { get; set; }
    public DateTimeOffset? ForgottenDate { get; set; }
    public bool IsDisabled { get; set; }
    public DateTimeOffset? DisabledDate { get; set; }
    public string DisabledReason { get; set; }

    public UserProfile Profile { get; set; }
    public IList<UserRole> Roles { get; private set; }
    public IList<MeetingRequest> MeetingRequests { get; private set; }
    public IList<MeetingChatMessage> MeetingChatMessages { get; private set; }

    public void Update()
    {
      ModifiedDate = DateTimeOffset.UtcNow;
    }

    public void UpdateLanguage(string language)
    {
      Language = language;
      Update();
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

using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Users;
using Skelvy.Domain.Exceptions;

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
      Messages = new List<Message>();
      MeetingRequests = new List<MeetingRequest>();
    }

    public User(string language)
    {
      Language = language;

      CreatedAt = DateTimeOffset.UtcNow;
      Roles = new List<UserRole>();
      Messages = new List<Message>();
      MeetingRequests = new List<MeetingRequest>();
    }

    public User(
      int id,
      string email,
      string language,
      string facebookId,
      string googleId,
      DateTimeOffset createdAt,
      DateTimeOffset? modifiedAt,
      bool isRemoved,
      DateTimeOffset? forgottenAt,
      bool isDisabled,
      string disabledReason,
      UserProfile profile,
      IList<UserRole> roles,
      IList<MeetingRequest> meetingRequests,
      IList<Message> messages)
    {
      Id = id;
      Email = email;
      Language = language;
      FacebookId = facebookId;
      GoogleId = googleId;
      CreatedAt = createdAt;
      ModifiedAt = modifiedAt;
      IsRemoved = isRemoved;
      ForgottenAt = forgottenAt;
      IsDisabled = isDisabled;
      DisabledReason = disabledReason;
      Profile = profile;
      Roles = roles;
      MeetingRequests = meetingRequests;
      Messages = messages;
    }

    public int Id { get; private set; }
    public string Email { get; private set; }
    public string Language { get; private set; }
    public string FacebookId { get; private set; }
    public string GoogleId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ModifiedAt { get; private set; }
    public bool IsRemoved { get; private set; }
    public DateTimeOffset? ForgottenAt { get; private set; }
    public bool IsDisabled { get; private set; }
    public string DisabledReason { get; private set; }

    public UserProfile Profile { get; private set; }
    public IList<UserRole> Roles { get; private set; }
    public IList<MeetingRequest> MeetingRequests { get; private set; }
    public IList<Message> Messages { get; private set; }
    public IList<Relation> Relations { get; private set; }
    public IList<FriendRequest> FriendsRequests { get; private set; }

    public void RegisterFacebook(string facebookId)
    {
      if (FacebookId == null)
      {
        FacebookId = facebookId ??
                     throw new DomainException($"'FacebookId' must not be null for entity {nameof(User)}(Id = {Id}).");

        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(User)}(Id = {Id}) has already connected facebook account.");
      }
    }

    public void RegisterGoogle(string googleId)
    {
      if (GoogleId == null)
      {
        GoogleId = googleId ?? throw new DomainException(
                     $"'GoogleId' must not be null for entity {nameof(User)}(Id = {Id}).");

        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(User)}(Id = {Id}) has already connected google account.");
      }
    }

    public void UpdateLanguage(string language)
    {
      if (language != Language)
      {
        Language = language == LanguageTypes.EN || language == LanguageTypes.PL
          ? language
          : throw new DomainException(
            $"'Language' must be {LanguageTypes.PL} or {LanguageTypes.EN} for entity {nameof(UserProfile)}(Id = {Id}).");

        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(User)}(Id = {Id}) has set current language.");
      }
    }

    public void Remove(DateTimeOffset forgottenAt)
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        ForgottenAt = forgottenAt;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(User)}(Id = {Id}) is already removed.");
      }
    }

    public void Disable(string reason)
    {
      if (!IsDisabled)
      {
        IsDisabled = true;
        DisabledReason =
          reason ?? throw new DomainException(
            $"'DisabledReason' must not be null for entity {nameof(User)}(Id = {Id}).");

        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(User)}(Id = {Id}) is already disabled.");
      }
    }
  }
}

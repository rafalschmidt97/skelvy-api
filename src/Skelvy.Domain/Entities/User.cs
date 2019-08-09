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
    }

    public User(string language)
    {
      Language = language;

      CreatedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; set; }
    public string Email { get; set; }
    public string Language { get; set; }
    public string FacebookId { get; set; }
    public string GoogleId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public bool IsRemoved { get; set; }
    public DateTimeOffset? ForgottenAt { get; set; }
    public bool IsDisabled { get; set; }
    public string DisabledReason { get; set; }

    public UserProfile Profile { get; set; }
    public IList<UserRole> Roles { get; set; }
    public IList<MeetingRequest> MeetingRequests { get; set; }
    public IList<Message> Messages { get; set; }
    public IList<Relation> Relations { get; set; }
    public IList<FriendRequest> FriendsRequests { get; set; }

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

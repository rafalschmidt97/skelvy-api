using System;
using System.Collections.Generic;
using System.Globalization;
using Skelvy.Domain.Entities.Core;
using Skelvy.Domain.Enums;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class User : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public User(string email, string name, string language)
    {
      Email = email;
      Name = name;
      Language = language;

      CreatedAt = DateTimeOffset.UtcNow;
    }

    public User(string name, string language)
    {
      Name = name;
      Language = language;

      CreatedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Language { get; set; }
    public string FacebookId { get; set; }
    public string GoogleId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public bool IsRemoved { get; set; }
    public DateTimeOffset? ForgottenAt { get; set; }
    public bool IsDisabled { get; set; }
    public string DisabledReason { get; set; }

    public Profile Profile { get; set; }
    public IList<UserRole> Roles { get; set; }
    public IList<RefreshToken> RefreshTokens { get; set; }
    public IList<MeetingRequest> MeetingRequests { get; set; }
    public IList<Message> Messages { get; set; }
    public IList<Relation> Relations { get; set; }
    public IList<FriendInvitation> FriendInvitations { get; set; }
    public IList<MeetingInvitation> MeetingInvitations { get; set; }

    public void RegisterFacebook(string facebookId)
    {
      if (FacebookId == null)
      {
        FacebookId = facebookId ??
                     throw new DomainException($"'FacebookId' must not be empty for {nameof(User)}({Id}).");

        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"{nameof(User)}({Id}) has already connected Facebook account.");
      }
    }

    public void RegisterGoogle(string googleId)
    {
      if (GoogleId == null)
      {
        GoogleId = googleId ?? throw new DomainException(
                     $"'GoogleId' must not be empty for {nameof(User)}({Id}).");

        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"{nameof(User)}({Id}) has already connected Google account.");
      }
    }

    public void UpdateLanguage(string language)
    {
      Language = LanguageType.Check(language)
        ? language
        : throw new DomainException(LanguageType.CheckFailedResponse());

      ModifiedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateName(string name)
    {
      Name = name.Trim().ToLower(CultureInfo.CurrentCulture);
      ModifiedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateEmail(string email)
    {
      Email = email.Trim().ToLower(CultureInfo.CurrentCulture);
      ModifiedAt = DateTimeOffset.UtcNow;
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
        throw new DomainException($"{nameof(User)}({Id}) is already removed.");
      }
    }

    public void Disable(string reason)
    {
      if (!IsDisabled)
      {
        IsDisabled = true;
        DisabledReason =
          reason ?? throw new DomainException(
            $"'DisabledReason' must not be empty for {nameof(User)}({Id}).");

        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"{nameof(User)}({Id}) is already disabled.");
      }
    }
  }
}

using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Users;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class UserProfile : IModifiableEntity
  {
    public UserProfile(string name, DateTimeOffset birthday, string gender, int userId)
    {
      Name = name.Trim();
      Birthday = birthday;
      Gender = gender;
      UserId = userId;

      Photos = new List<UserProfilePhoto>();
    }

    public UserProfile(
      int id,
      string name,
      DateTimeOffset birthday,
      string gender,
      string description,
      DateTimeOffset? modifiedAt,
      int userId,
      IList<UserProfilePhoto> photos,
      User user)
    {
      Id = id;
      Name = name;
      Birthday = birthday;
      Gender = gender;
      Description = description;
      ModifiedAt = modifiedAt;
      UserId = userId;
      Photos = photos;
      User = user;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public DateTimeOffset Birthday { get; private set; }
    public string Gender { get; private set; }
    public string Description { get; private set; }
    public DateTimeOffset? ModifiedAt { get; private set; }
    public int UserId { get; private set; }

    public IList<UserProfilePhoto> Photos { get; private set; }
    public User User { get; private set; }

    public void Update(string name, DateTimeOffset birthday, string gender, string description)
    {
      Name = name != null
        ? name.Trim()
        : throw new DomainException($"'Name' must not be null for entity {nameof(UserProfile)}(Id = {Id}).");

      Birthday = birthday <= DateTimeOffset.UtcNow.AddYears(-18)
        ? birthday
        : throw new DomainException(
          $"'Birthday' must show the age of majority for entity {nameof(UserProfile)}(Id = {Id}).");

      Gender = gender == GenderTypes.Male || gender == GenderTypes.Female || gender == GenderTypes.Other
        ? gender
        : throw new DomainException(
          $"'Gender' must be {GenderTypes.Male} / {GenderTypes.Female} / {GenderTypes.Other} for entity {nameof(UserProfile)}(Id = {Id}).");

      if (description != null)
      {
        Description = description.Trim();
      }

      ModifiedAt = DateTimeOffset.UtcNow;
    }
  }
}

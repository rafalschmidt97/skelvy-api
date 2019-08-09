using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Users;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class Profile : IModifiableEntity
  {
    public Profile(string name, DateTimeOffset birthday, string gender, int userId)
    {
      Name = name.Trim();
      Birthday = birthday;
      Gender = gender;
      UserId = userId;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset Birthday { get; set; }
    public string Gender { get; set; }
    public string Description { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public int UserId { get; set; }

    public IList<ProfilePhoto> Photos { get; set; }
    public User User { get; set; }

    public void Update(string name, DateTimeOffset birthday, string gender, string description)
    {
      Name = name != null
        ? name.Trim()
        : throw new DomainException($"'Name' must not be null for entity {nameof(Profile)}(Id = {Id}).");

      Birthday = birthday <= DateTimeOffset.UtcNow.AddYears(-18)
        ? birthday
        : throw new DomainException(
          $"'Birthday' must show the age of majority for entity {nameof(Profile)}(Id = {Id}).");

      Gender = gender == GenderTypes.Male || gender == GenderTypes.Female || gender == GenderTypes.Other
        ? gender
        : throw new DomainException(
          $"'Gender' must be {GenderTypes.Male} / {GenderTypes.Female} / {GenderTypes.Other} for entity {nameof(Profile)}(Id = {Id}).");

      if (description != null)
      {
        Description = description.Trim();
      }

      ModifiedAt = DateTimeOffset.UtcNow;
    }
  }
}

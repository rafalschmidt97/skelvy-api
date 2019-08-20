using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Core;
using Skelvy.Domain.Enums;
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
        : throw new DomainException($"'Name' must not be empty for {nameof(Profile)}({Id}).");

      Birthday = birthday <= DateTimeOffset.UtcNow.AddYears(-18)
        ? birthday
        : throw new DomainException(
          $"'Birthday' must show the age of majority for {nameof(Profile)}({Id}).");

      Gender = gender == GenderType.Male || gender == GenderType.Female || gender == GenderType.Other
        ? gender
        : throw new DomainException(
          $"'Gender' must be {GenderType.Male} / {GenderType.Female} / {GenderType.Other} for {nameof(Profile)}({Id}).");

      Description = description?.Trim();

      ModifiedAt = DateTimeOffset.UtcNow;
    }
  }
}

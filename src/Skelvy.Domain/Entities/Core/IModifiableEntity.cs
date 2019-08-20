using System;

namespace Skelvy.Domain.Entities.Core
{
  public interface IModifiableEntity
  {
    DateTimeOffset? ModifiedAt { get; }
  }
}

using System;

namespace Skelvy.Domain.Entities.Base
{
  public interface IModifiableEntity
  {
    DateTimeOffset? ModifiedAt { get; }
  }
}

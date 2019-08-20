using System;

namespace Skelvy.Domain.Entities.Core
{
  public interface ICreatableEntity
  {
    DateTimeOffset CreatedAt { get; }
  }
}

using System;

namespace Skelvy.Domain.Entities.Base
{
  public interface IRemovableEntity
  {
    bool IsRemoved { get; }
    DateTimeOffset? RemovedAt { get; }
  }
}

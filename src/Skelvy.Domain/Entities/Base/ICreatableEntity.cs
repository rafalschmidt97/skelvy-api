using System;

namespace Skelvy.Domain.Entities.Base
{
  public interface ICreatableEntity
  {
    DateTimeOffset CreatedDate { get; }
  }
}

using System;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;
using Skelvy.Domain.Exceptions;
using Xunit;

namespace Skelvy.Domain.Test.Entities
{
  public class UserProfileTest
  {
    [Fact]
    public void ShouldBeUpdatedWithoutDescription()
    {
      var entity = new UserProfile("Example", DateTimeOffset.UtcNow.AddYears(-18), GenderTypes.Male, 1);
      entity.Update("Example2 ", DateTimeOffset.UtcNow.AddYears(-19), GenderTypes.Female, null);

      Assert.Equal("Example2", entity.Name);
      Assert.Null(entity.Description);
      Assert.NotNull(entity.ModifiedAt);
    }

    [Fact]
    public void ShouldBeUpdatedWithDescription()
    {
      var entity = new UserProfile("Example", DateTimeOffset.UtcNow.AddYears(-18), GenderTypes.Male, 1);
      entity.Update("Example2 ", DateTimeOffset.UtcNow.AddYears(-19), GenderTypes.Female, " Description ");

      Assert.Equal("Example2", entity.Name);
      Assert.Equal("Description", entity.Description);
      Assert.NotNull(entity.ModifiedAt);
    }

    [Fact]
    public void ShouldThrowExceptionWithUpdatingNullProperty()
    {
      var entity = new UserProfile("Example", DateTimeOffset.UtcNow.AddYears(-18), GenderTypes.Male, 1);

      Assert.Throws<DomainException>(() =>
          entity.Update(null, DateTimeOffset.UtcNow.AddYears(-19), null, null));
    }
  }
}

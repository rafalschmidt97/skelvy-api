using System;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;
using Xunit;

namespace Skelvy.Domain.Test.Entities
{
  public class UserTest
  {
    [Fact]
    public void ShouldHasConnectedFacebook()
    {
      var entity = new User("example@gmail.com", LanguageTypes.EN);
      entity.RegisterFacebook("facebook1");

      Assert.NotNull(entity.FacebookId);
      Assert.Equal("facebook1", entity.FacebookId);
    }

    [Fact]
    public void ShouldHasConnectedGoogle()
    {
      var entity = new User("example@gmail.com", LanguageTypes.EN);
      entity.RegisterGoogle("google1");

      Assert.NotNull(entity.GoogleId);
      Assert.Equal("google1", entity.GoogleId);
    }

    [Fact]
    public void ShouldHasUpdatedLanguage()
    {
      var entity = new User("example@gmail.com", LanguageTypes.EN);
      entity.UpdateLanguage(LanguageTypes.PL);

      Assert.Equal(LanguageTypes.PL, entity.Language);
      Assert.NotNull(entity.ModifiedAt);
    }

    [Fact]
    public void ShouldBeRemoved()
    {
      var entity = new User("example@gmail.com", LanguageTypes.EN);
      entity.Remove(DateTimeOffset.UtcNow);

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.RemovedAt);
      Assert.NotNull(entity.ForgottenAt);
    }

    [Fact]
    public void ShouldBeDisabled()
    {
      var entity = new User("example@gmail.com", LanguageTypes.EN);
      entity.Disable("Test");

      Assert.True(entity.IsDisabled);
      Assert.NotNull(entity.DisabledAt);
      Assert.NotNull(entity.DisabledReason);
    }
  }
}

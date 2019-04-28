using System;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;
using Skelvy.Domain.Exceptions;
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
    public void ShouldThrowExceptionWithConnectedFacebook()
    {
      var entity = new User("example@gmail.com", LanguageTypes.EN);
      entity.RegisterFacebook("facebook1");

      Assert.Throws<DomainException>(() =>
        entity.RegisterFacebook("facebook1"));
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
    public void ShouldThrowExceptionWithConnectedGoogle()
    {
      var entity = new User("example@gmail.com", LanguageTypes.EN);
      entity.RegisterGoogle("google1");

      Assert.Throws<DomainException>(() =>
        entity.RegisterGoogle("google1"));
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
    public void ShouldThrowExceptionWithUpdatingSameLanguage()
    {
      var entity = new User("example@gmail.com", LanguageTypes.EN);

      Assert.Throws<DomainException>(() =>
        entity.UpdateLanguage(LanguageTypes.EN));
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
    public void ShouldThrowExceptionWithRemoved()
    {
      var entity = new User("example@gmail.com", LanguageTypes.EN);
      entity.Remove(DateTimeOffset.UtcNow);

      Assert.Throws<DomainException>(() =>
        entity.Remove(DateTimeOffset.UtcNow));
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

    [Fact]
    public void ShouldThrowExceptionWithDisabled()
    {
      var entity = new User("example@gmail.com", LanguageTypes.EN);
      entity.Disable("Test");

      Assert.Throws<DomainException>(() =>
        entity.Disable("Test"));
    }
  }
}

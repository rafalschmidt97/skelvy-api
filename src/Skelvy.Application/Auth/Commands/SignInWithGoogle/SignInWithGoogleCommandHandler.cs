using System;
using System.Globalization;
using System.Threading.Tasks;
using Skelvy.Application.Auth.Infrastructure.Google;
using Skelvy.Application.Auth.Infrastructure.Repositories;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Auth.Commands.SignInWithGoogle
{
  public class SignInWithGoogleCommandHandler : QueryHandler<SignInWithGoogleCommand, AuthDto>
  {
    private readonly IAuthRepository _repository;
    private readonly IGoogleService _googleService;
    private readonly ITokenService _tokenService;
    private readonly INotificationsService _notifications;

    public SignInWithGoogleCommandHandler(
      IAuthRepository repository,
      IGoogleService googleService,
      ITokenService tokenService,
      INotificationsService notifications)
    {
      _repository = repository;
      _googleService = googleService;
      _tokenService = tokenService;
      _notifications = notifications;
    }

    public override async Task<AuthDto> Handle(SignInWithGoogleCommand request)
    {
      var verified = await _googleService.Verify(request.AuthToken);

      var user = await _repository.FindOneWithRolesByGoogleId(verified.UserId);

      if (user == null)
      {
        var details = await _googleService.GetBody<dynamic>(
          "plus/v1/people/me",
          request.AuthToken,
          "fields=birthday,name/givenName,emails/value,gender,image/url");

        var email = (string)details.emails[0].value;
        var userByEmail = await _repository.FindOneWithRolesByEmail(email);

        var isDataChanged = false;

        if (userByEmail == null)
        {
          user = new User((string)details.emails[0].value, request.Language);
          user.RegisterGoogle(verified.UserId);
          _repository.Context.Users.Add(user);

          var birthday = details.birthday != null
            ? DateTimeOffset.ParseExact(
              (string)details.birthday,
              "yyyy-MM-dd",
              CultureInfo.CurrentCulture).ToUniversalTime()
            : DateTimeOffset.UtcNow;

          var profile = new UserProfile(
            (string)details.name.givenName,
            birthday <= DateTimeOffset.UtcNow.AddYears(-18) ? birthday : DateTimeOffset.UtcNow.AddYears(-18),
            details.gender == GenderTypes.Female ? GenderTypes.Female : GenderTypes.Male,
            user.Id);

          _repository.Context.UserProfiles.Add(profile);

          if (details.image != null)
          {
            var photo = new UserProfilePhoto((string)details.image.url, profile.Id);
            _repository.Context.UserProfilePhotos.Add(photo);
          }
        }
        else
        {
          if (userByEmail.IsRemoved)
          {
            throw new UnauthorizedException("User is in safety retention window for deletion");
          }

          if (userByEmail.IsDisabled)
          {
            throw new UnauthorizedException("User is disabled");
          }

          userByEmail.RegisterGoogle(verified.UserId);
          user = userByEmail;
          isDataChanged = true;
        }

        await _repository.Context.SaveChangesAsync();

        if (!isDataChanged)
        {
          await _notifications.BroadcastUserCreated(user);
        }
      }
      else
      {
        if (user.IsRemoved)
        {
          throw new UnauthorizedException("User is in safety retention window for deletion");
        }

        if (user.IsDisabled)
        {
          throw new UnauthorizedException("User is disabled");
        }
      }

      return await _tokenService.Generate(user);
    }
  }
}

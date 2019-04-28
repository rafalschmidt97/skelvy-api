using System;
using System.Globalization;
using System.Threading.Tasks;
using Skelvy.Application.Auth.Infrastructure.Google;
using Skelvy.Application.Auth.Infrastructure.Repositories;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Auth.Commands.SignInWithGoogle
{
  public class SignInWithGoogleCommandHandler : QueryHandler<SignInWithGoogleCommand, AuthDto>
  {
    private readonly IAuthRepository _authRepository;
    private readonly IUserProfilesRepository _profilesRepository;
    private readonly IUserProfilePhotosRepository _profilePhotosRepository;
    private readonly IGoogleService _googleService;
    private readonly ITokenService _tokenService;
    private readonly INotificationsService _notifications;

    public SignInWithGoogleCommandHandler(
      IAuthRepository authRepository,
      IUserProfilesRepository profilesRepository,
      IUserProfilePhotosRepository profilePhotosRepository,
      IGoogleService googleService,
      ITokenService tokenService,
      INotificationsService notifications)
    {
      _authRepository = authRepository;
      _profilesRepository = profilesRepository;
      _profilePhotosRepository = profilePhotosRepository;
      _googleService = googleService;
      _tokenService = tokenService;
      _notifications = notifications;
    }

    public override async Task<AuthDto> Handle(SignInWithGoogleCommand request)
    {
      var verified = await _googleService.Verify(request.AuthToken);

      var user = await _authRepository.FindOneWithRolesByGoogleId(verified.UserId);

      if (user == null)
      {
        var details = await _googleService.GetBody<dynamic>(
          "plus/v1/people/me",
          request.AuthToken,
          "fields=birthday,name/givenName,emails/value,gender,image/url");

        var email = (string)details.emails[0].value;
        var userByEmail = await _authRepository.FindOneWithRolesByEmail(email);

        var isDataChanged = false;

        if (userByEmail == null)
        {
          user = new User((string)details.emails[0].value, request.Language);
          user.RegisterGoogle(verified.UserId);
          _authRepository.AddAsTransaction(user);

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

          _profilesRepository.AddAsTransaction(profile);

          if (details.image != null)
          {
            var photo = new UserProfilePhoto((string)details.image.url, profile.Id);
            _profilePhotosRepository.AddAsTransaction(photo);
          }

          await _authRepository.Commit();
        }
        else
        {
          ValidateUser(userByEmail);
          userByEmail.RegisterGoogle(verified.UserId);
          await _authRepository.Update(userByEmail);

          user = userByEmail;
          isDataChanged = true;
        }

        if (!isDataChanged)
        {
          await _notifications.BroadcastUserCreated(user);
        }
      }
      else
      {
        ValidateUser(user);
      }

      return await _tokenService.Generate(user);
    }

    private static void ValidateUser(User user)
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
  }
}

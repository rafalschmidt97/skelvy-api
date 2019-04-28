using System;
using System.Globalization;
using System.Threading.Tasks;
using Skelvy.Application.Auth.Infrastructure.Facebook;
using Skelvy.Application.Auth.Infrastructure.Repositories;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Auth.Commands.SignInWithFacebook
{
  public class SignInWithFacebookCommandHandler : QueryHandler<SignInWithFacebookCommand, AuthDto>
  {
    private readonly IAuthRepository _authRepository;
    private readonly IUserProfilesRepository _profilesRepository;
    private readonly IUserProfilePhotosRepository _profilePhotosRepository;
    private readonly IFacebookService _facebookService;
    private readonly ITokenService _tokenService;
    private readonly INotificationsService _notifications;

    public SignInWithFacebookCommandHandler(
      IAuthRepository authRepository,
      IUserProfilesRepository profilesRepository,
      IUserProfilePhotosRepository profilePhotosRepository,
      IFacebookService facebookService,
      ITokenService tokenService,
      INotificationsService notifications)
    {
      _authRepository = authRepository;
      _profilesRepository = profilesRepository;
      _profilePhotosRepository = profilePhotosRepository;
      _facebookService = facebookService;
      _tokenService = tokenService;
      _notifications = notifications;
    }

    public override async Task<AuthDto> Handle(SignInWithFacebookCommand request)
    {
      var verified = await _facebookService.Verify(request.AuthToken);

      var user = await _authRepository.FindOneWithRolesByFacebookId(verified.UserId);

      if (user == null)
      {
        var details = await _facebookService.GetBody<dynamic>(
          "me",
          request.AuthToken,
          "fields=birthday,email,first_name,gender,picture.width(512).height(512){url}");

        var email = (string)details.email;
        var userByEmail = await _authRepository.FindOneWithRolesByEmail(email);
        var isDataChanged = false;

        if (userByEmail == null)
        {
          user = new User((string)details.email, request.Language);
          user.RegisterFacebook(verified.UserId);
          _authRepository.AddAsTransaction(user);

          var birthday = DateTimeOffset.ParseExact(
            (string)details.birthday,
            "MM/dd/yyyy",
            CultureInfo.CurrentCulture).ToUniversalTime();

          var profile = new UserProfile(
            (string)details.first_name,
            birthday <= DateTimeOffset.UtcNow.AddYears(-18) ? birthday : DateTimeOffset.UtcNow.AddYears(-18),
            details.gender == GenderTypes.Female ? GenderTypes.Female : GenderTypes.Male,
            user.Id);

          _profilesRepository.AddAsTransaction(profile);

          if (details.picture != null)
          {
            var photo = new UserProfilePhoto((string)details.picture.data.url, profile.Id);
            _profilePhotosRepository.AddAsTransaction(photo);
          }

          await _authRepository.Commit();
        }
        else
        {
          ValidateUser(userByEmail);
          userByEmail.RegisterFacebook(verified.UserId);
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

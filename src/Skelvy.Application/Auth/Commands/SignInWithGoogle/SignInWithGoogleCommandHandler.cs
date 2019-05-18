using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Skelvy.Application.Auth.Infrastructure.Google;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Application.Users.Infrastructure.Notifications;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Auth.Commands.SignInWithGoogle
{
  public class SignInWithGoogleCommandHandler : QueryHandler<SignInWithGoogleCommand, AuthDto>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IUserProfilesRepository _profilesRepository;
    private readonly IUserProfilePhotosRepository _profilePhotosRepository;
    private readonly IGoogleService _googleService;
    private readonly ITokenService _tokenService;
    private readonly INotificationsService _notifications;
    private readonly ILogger<SignInWithGoogleCommandHandler> _logger;

    public SignInWithGoogleCommandHandler(
      IUsersRepository usersRepository,
      IUserProfilesRepository profilesRepository,
      IUserProfilePhotosRepository profilePhotosRepository,
      IGoogleService googleService,
      ITokenService tokenService,
      INotificationsService notifications,
      ILogger<SignInWithGoogleCommandHandler> logger)
    {
      _usersRepository = usersRepository;
      _profilesRepository = profilesRepository;
      _profilePhotosRepository = profilePhotosRepository;
      _googleService = googleService;
      _tokenService = tokenService;
      _notifications = notifications;
      _logger = logger;
    }

    public override async Task<AuthDto> Handle(SignInWithGoogleCommand request)
    {
      var verified = await _googleService.Verify(request.AuthToken);

      var user = await _usersRepository.FindOneWithRolesByGoogleId(verified.UserId);

      if (user == null)
      {
        var details = await _googleService.GetBody<dynamic>(
          "plus/v1/people/me",
          request.AuthToken,
          "fields=birthday,name/givenName,emails/value,gender,image/url");

        var email = (string)details.emails[0].value;
        var userByEmail = await _usersRepository.FindOneWithRolesByEmail(email);

        var isDataChanged = false;

        if (userByEmail == null)
        {
          using (var transaction = _usersRepository.BeginTransaction())
          {
            user = new User((string)details.emails[0].value, request.Language);
            user.RegisterGoogle(verified.UserId);
            _logger.LogInformation("Adding User from: {details} = {@User}", (string)JsonConvert.SerializeObject(details), user);
            await _usersRepository.Add(user);

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

            await _profilesRepository.Add(profile);

            if (details.image != null)
            {
              var photo = new UserProfilePhoto((string)details.image.url, 1, profile.Id);
              await _profilePhotosRepository.Add(photo);
            }

            transaction.Commit();
          }
        }
        else
        {
          ValidateUser(userByEmail);
          userByEmail.RegisterGoogle(verified.UserId);
          await _usersRepository.Update(userByEmail);

          user = userByEmail;
          isDataChanged = true;
        }

        if (!isDataChanged)
        {
          await _notifications.BroadcastUserCreated(new UserCreatedAction(user.Id, user.Email, user.Language));
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

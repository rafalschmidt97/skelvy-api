using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Skelvy.Application.Auth.Infrastructure.Facebook;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Application.Users.Infrastructure.Notifications;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Auth.Commands.SignInWithFacebook
{
  public class SignInWithFacebookCommandHandler : QueryHandler<SignInWithFacebookCommand, AuthDto>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IUserProfilesRepository _profilesRepository;
    private readonly IUserProfilePhotosRepository _profilePhotosRepository;
    private readonly IFacebookService _facebookService;
    private readonly ITokenService _tokenService;
    private readonly INotificationsService _notifications;
    private readonly ILogger<SignInWithFacebookCommandHandler> _logger;

    public SignInWithFacebookCommandHandler(
      IUsersRepository usersRepository,
      IUserProfilesRepository profilesRepository,
      IUserProfilePhotosRepository profilePhotosRepository,
      IFacebookService facebookService,
      ITokenService tokenService,
      INotificationsService notifications,
      ILogger<SignInWithFacebookCommandHandler> logger)
    {
      _usersRepository = usersRepository;
      _profilesRepository = profilesRepository;
      _profilePhotosRepository = profilePhotosRepository;
      _facebookService = facebookService;
      _tokenService = tokenService;
      _notifications = notifications;
      _logger = logger;
    }

    public override async Task<AuthDto> Handle(SignInWithFacebookCommand request)
    {
      var verified = await _facebookService.Verify(request.AuthToken);
      var accountCreated = false;
      var user = await _usersRepository.FindOneWithRolesByFacebookId(verified.UserId);

      if (user == null)
      {
        var details = await _facebookService.GetBody<dynamic>(
          "me",
          request.AuthToken,
          "fields=birthday,email,first_name,gender,picture.width(512).height(512){url}");

        var email = (string)details.email;

        if (email == null)
        {
          user = new User("", request.Language);
          user.RegisterFacebook(verified.UserId);
          await CreateUserWithProfile(user, details);

          accountCreated = true;
        }
        else
        {
          var userByEmail = await _usersRepository.FindOneWithRolesByEmail(email);
          var accountChanged = false;

          if (userByEmail == null)
          {
            if (email.Length > 50)
            {
              throw new UnauthorizedException($"Entity {nameof(User)}(FacebookId = {verified.UserId}, Email = {email}) has too long email");
            }

            user = new User(email, request.Language);
            user.RegisterFacebook(verified.UserId);
            await CreateUserWithProfile(user, details);

            accountCreated = true;
          }
          else
          {
            ValidateUser(userByEmail);
            userByEmail.RegisterFacebook(verified.UserId);
            await _usersRepository.Update(userByEmail);

            user = userByEmail;
            accountChanged = true;
          }

          if (!accountChanged)
          {
            await _notifications.BroadcastUserCreated(new UserCreatedAction(user.Id, user.Email, user.Language));
          }
        }
      }
      else
      {
        ValidateUser(user);
      }

      var token = await _tokenService.Generate(user);

      return new AuthDto
      {
        AccountCreated = accountCreated,
        AccessToken = token.AccessToken,
        RefreshToken = token.RefreshToken,
      };
    }

    private async Task CreateUserWithProfile(User user, dynamic details)
    {
      using (var transaction = _usersRepository.BeginTransaction())
      {
        _logger.LogInformation(
          "Adding User from: {details} = {@User}",
          (string)JsonConvert.SerializeObject(details),
          user);

        await _usersRepository.Add(user);

        var birthday = details.birthday != null
          ? DateTimeOffset.ParseExact(
            (string)details.birthday,
            "MM/dd/yyyy",
            CultureInfo.CurrentCulture).ToUniversalTime()
          : DateTimeOffset.UtcNow;

        var profile = new UserProfile(
          (string)details.first_name,
          birthday <= DateTimeOffset.UtcNow.AddYears(-18) ? birthday : DateTimeOffset.UtcNow.AddYears(-18),
          details.gender == GenderTypes.Female ? GenderTypes.Female : GenderTypes.Male,
          user.Id);

        await _profilesRepository.Add(profile);

        if (details.picture != null)
        {
          var photo = new UserProfilePhoto((string)details.picture.data.url, 1, profile.Id);
          await _profilePhotosRepository.Add(photo);
        }

        transaction.Commit();
      }
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

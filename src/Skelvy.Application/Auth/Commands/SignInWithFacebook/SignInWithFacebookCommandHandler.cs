using System;
using System.Globalization;
using System.Threading.Tasks;
using Skelvy.Application.Auth.Infrastructure.Facebook;
using Skelvy.Application.Auth.Infrastructure.Repositories;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Auth.Commands.SignInWithFacebook
{
  public class SignInWithFacebookCommandHandler : QueryHandler<SignInWithFacebookCommand, AuthDto>
  {
    private readonly IAuthRepository _repository;
    private readonly IFacebookService _facebookService;
    private readonly ITokenService _tokenService;
    private readonly INotificationsService _notifications;

    public SignInWithFacebookCommandHandler(
      IAuthRepository repository,
      IFacebookService facebookService,
      ITokenService tokenService,
      INotificationsService notifications)
    {
      _repository = repository;
      _facebookService = facebookService;
      _tokenService = tokenService;
      _notifications = notifications;
    }

    public override async Task<AuthDto> Handle(SignInWithFacebookCommand request)
    {
      var verified = await _facebookService.Verify(request.AuthToken);

      var user = await _repository.FindOneWithRolesByFacebookId(verified.UserId);

      if (user == null)
      {
        var details = await _facebookService.GetBody<dynamic>(
          "me",
          request.AuthToken,
          "fields=birthday,email,first_name,gender,picture.width(512).height(512){url}");

        var email = (string)details.email;
        var userByEmail = await _repository.FindOneWithRolesByEmail(email);
        var isDataChanged = false;

        if (userByEmail == null)
        {
          user = new User((string)details.email, request.Language);
          user.RegisterFacebook(verified.UserId);
          _repository.Context.Users.Add(user);

          var birthday = DateTimeOffset.ParseExact(
            (string)details.birthday,
            "MM/dd/yyyy",
            CultureInfo.CurrentCulture).ToUniversalTime();

          var profile = new UserProfile(
            (string)details.first_name,
            birthday <= DateTimeOffset.UtcNow.AddYears(-18) ? birthday : DateTimeOffset.UtcNow.AddYears(-18),
            details.gender == GenderTypes.Female ? GenderTypes.Female : GenderTypes.Male,
            user.Id);

          _repository.Context.UserProfiles.Add(profile);

          if (details.picture != null)
          {
            var photo = new UserProfilePhoto((string)details.picture.data.url, profile.Id);
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

          userByEmail.RegisterFacebook(verified.UserId);
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

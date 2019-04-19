using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Auth.Infrastructure.Facebook;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Application.Users.Commands;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Auth.Commands.SignInWithFacebook
{
  public class SignInWithFacebookCommandHandler : QueryHandler<SignInWithFacebookCommand, Token>
  {
    private readonly SkelvyContext _context;
    private readonly IFacebookService _facebookService;
    private readonly ITokenService _tokenService;
    private readonly INotificationsService _notifications;

    public SignInWithFacebookCommandHandler(
      SkelvyContext context,
      IFacebookService facebookService,
      ITokenService tokenService,
      INotificationsService notifications)
    {
      _context = context;
      _facebookService = facebookService;
      _tokenService = tokenService;
      _notifications = notifications;
    }

    public override async Task<Token> Handle(SignInWithFacebookCommand request)
    {
      var verified = await _facebookService.Verify(request.AuthToken);

      var user = await _context.Users
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.FacebookId == verified.UserId);

      if (user == null)
      {
        var details = await _facebookService.GetBody<dynamic>(
          "me",
          request.AuthToken,
          "fields=birthday,email,first_name,gender,picture.width(512).height(512){url}");

        var email = (string)details.email;
        var userByEmail = await _context.Users
          .Include(x => x.Roles)
          .FirstOrDefaultAsync(x => x.Email == email);

        var isDataChanged = false;

        if (userByEmail == null)
        {
          user = new User
          {
            Email = details.email,
            Language = request.Language,
            FacebookId = verified.UserId,
            IsRemoved = false,
            IsDisabled = false,
          };
          _context.Users.Add(user);

          var birthday = DateTimeOffset.ParseExact(
            (string)details.birthday,
            "MM/dd/yyyy",
            CultureInfo.CurrentCulture).ToUniversalTime();

          var profile = new UserProfile
          {
            Name = details.first_name,
            Birthday = birthday <= DateTimeOffset.UtcNow.AddYears(-18) ? birthday : DateTimeOffset.UtcNow.AddYears(-18),
            Gender = details.gender == GenderTypes.Female ? GenderTypes.Female : GenderTypes.Male,
            UserId = user.Id,
          };

          _context.UserProfiles.Add(profile);

          if (details.picture != null)
          {
            var photo = new UserProfilePhoto
            {
              Url = details.picture.data.url,
              ProfileId = profile.Id,
            };

            _context.UserProfilePhotos.Add(photo);
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

          userByEmail.FacebookId = verified.UserId;
          user = userByEmail;
          isDataChanged = true;
        }

        await _context.SaveChangesAsync();

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

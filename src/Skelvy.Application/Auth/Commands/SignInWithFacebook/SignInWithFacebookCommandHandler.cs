using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Infrastructure.Facebook;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Application.Infrastructure.Tokens;
using Skelvy.Application.Users.Commands;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Auth.Commands.SignInWithFacebook
{
  public class SignInWithFacebookCommandHandler : IRequestHandler<SignInWithFacebookCommand, string>
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

    public async Task<string> Handle(SignInWithFacebookCommand request, CancellationToken cancellationToken)
    {
      var verified = await _facebookService.Verify(request.AuthToken, cancellationToken);

      var user = await _context.Users
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.FacebookId == verified.UserId, cancellationToken);

      if (user == null)
      {
        var details = await _facebookService.GetBody<dynamic>(
          "me",
          request.AuthToken,
          "fields=birthday,email,first_name,gender,picture.width(512).height(512){url}",
          cancellationToken);

        var email = (string)details.email;
        var userByEmail = await _context.Users
          .Include(x => x.Roles)
          .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        var isDataChanged = false;

        if (userByEmail == null)
        {
          user = new User
          {
            Email = details.email,
            Language = request.Language,
            FacebookId = verified.UserId
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
            UserId = user.Id
          };

          _context.UserProfiles.Add(profile);

          if (details.picture != null)
          {
            var photo = new UserProfilePhoto
            {
              Url = details.picture.data.url,
              Status = UserProfilePhotoStatusTypes.Active,
              ProfileId = profile.Id
            };

            _context.UserProfilePhotos.Add(photo);
          }
        }
        else
        {
          userByEmail.FacebookId = verified.UserId;
          user = userByEmail;
          isDataChanged = true;
        }

        await _context.SaveChangesAsync(cancellationToken);

        if (!isDataChanged)
        {
          await _notifications.BroadcastUserCreated(user, cancellationToken);
        }
      }

      return _tokenService.Generate(user);
    }
  }
}

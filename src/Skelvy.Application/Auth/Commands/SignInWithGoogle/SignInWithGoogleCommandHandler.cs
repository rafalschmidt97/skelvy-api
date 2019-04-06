using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Infrastructure.Google;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Application.Infrastructure.Tokens;
using Skelvy.Application.Users.Commands;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Auth.Commands.SignInWithGoogle
{
  public class SignInWithGoogleCommandHandler : IRequestHandler<SignInWithGoogleCommand, string>
  {
    private readonly SkelvyContext _context;
    private readonly IGoogleService _googleService;
    private readonly ITokenService _tokenService;
    private readonly INotificationsService _notifications;

    public SignInWithGoogleCommandHandler(
      SkelvyContext context,
      IGoogleService googleService,
      ITokenService tokenService,
      INotificationsService notifications)
    {
      _context = context;
      _googleService = googleService;
      _tokenService = tokenService;
      _notifications = notifications;
    }

    public async Task<string> Handle(SignInWithGoogleCommand request, CancellationToken cancellationToken)
    {
      var verified = await _googleService.Verify(request.AuthToken, cancellationToken);

      var user = await _context.Users
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.GoogleId == verified.UserId, cancellationToken);

      if (user == null)
      {
        var details = await _googleService.GetBody<dynamic>(
          "plus/v1/people/me",
          request.AuthToken,
          "fields=birthday,name/givenName,emails/value,gender,image/url",
          cancellationToken);

        var email = (string)details.emails[0].value;
        var userByEmail = await _context.Users
          .Include(x => x.Roles)
          .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        var isDataChanged = false;

        if (userByEmail == null)
        {
          user = new User
          {
            Email = details.emails[0].value,
            Language = request.Language,
            GoogleId = verified.UserId,
            IsDeleted = false,
            IsDisabled = false
          };
          _context.Users.Add(user);

          var birthday = details.birthday != null
            ? DateTimeOffset.ParseExact(
              (string)details.birthday,
              "yyyy-MM-dd",
              CultureInfo.CurrentCulture).ToUniversalTime()
            : DateTimeOffset.UtcNow;

          var profile = new UserProfile
          {
            Name = details.name.givenName,
            Birthday = birthday <= DateTimeOffset.UtcNow.AddYears(-18) ? birthday : DateTimeOffset.UtcNow.AddYears(-18),
            Gender = details.gender == GenderTypes.Female ? GenderTypes.Female : GenderTypes.Male,
            UserId = user.Id
          };

          _context.UserProfiles.Add(profile);

          if (details.image != null)
          {
            var photo = new UserProfilePhoto
            {
              Url = details.image.url,
              Status = UserProfilePhotoStatusTypes.Active,
              ProfileId = profile.Id
            };

            _context.UserProfilePhotos.Add(photo);
          }
        }
        else
        {
          if (userByEmail.IsDeleted || userByEmail.IsDisabled)
          {
            throw new UnauthorizedException("User is in safety retention window for deletion");
          }

          userByEmail.GoogleId = verified.UserId;
          user = userByEmail;
          isDataChanged = true;
        }

        await _context.SaveChangesAsync(cancellationToken);

        if (!isDataChanged)
        {
          await _notifications.BroadcastUserCreated(user, cancellationToken);
        }
      }
      else
      {
        if (user.IsDeleted || user.IsDisabled)
        {
          throw new UnauthorizedException("User is in safety retention window for deletion");
        }
      }

      return _tokenService.Generate(user);
    }
  }
}

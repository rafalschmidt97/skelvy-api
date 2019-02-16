using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Infrastructure.Google;
using Skelvy.Application.Core.Infrastructure.Tokens;
using Skelvy.Common;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Auth.Commands.SignInWithGoogle
{
  public class SignInWithGoogleCommandHandler : IRequestHandler<SignInWithGoogleCommand, string>
  {
    private readonly SkelvyContext _context;
    private readonly IGoogleService _googleService;
    private readonly ITokenService _tokenService;

    public SignInWithGoogleCommandHandler(
      SkelvyContext context,
      IGoogleService googleService,
      ITokenService tokenService)
    {
      _context = context;
      _googleService = googleService;
      _tokenService = tokenService;
    }

    public async Task<string> Handle(SignInWithGoogleCommand request, CancellationToken cancellationToken)
    {
      var verified = await _googleService.Verify(request.AuthToken);

      var user = await _context.Users.FirstOrDefaultAsync(x => x.GoogleId == verified.UserId, cancellationToken);

      if (user == null)
      {
        var details = await _googleService.GetBody<dynamic>(
          "plus/v1/people/me",
          request.AuthToken,
          "fields=birthday,name/givenName,emails/value,gender,image/url");

        var email = (string)details.emails[0].value;
        var userByEmail = await _context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (userByEmail == null)
        {
          user = new User
          {
            Email = details.emails[0].value,
            GoogleId = verified.UserId
          };
          _context.Users.Add(user);

          var profile = new UserProfile
          {
            Name = details.name.givenName,
            Birthday = DateTime.ParseExact(
              (string)details.birthday,
              "yyyy-MM-dd",
              CultureInfo.CurrentCulture),
            Gender = details.gender == GenderTypes.Female ? GenderTypes.Female : GenderTypes.Male,
            UserId = user.Id
          };

          _context.UserProfiles.Add(profile);

          if (details.image != null)
          {
            var photo = new UserProfilePhoto
            {
              Url = details.image.url,
              ProfileId = profile.Id
            };

            _context.UserProfilePhotos.Add(photo);
          }
        }
        else
        {
          userByEmail.GoogleId = verified.UserId;
          user = userByEmail;
        }

        await _context.SaveChangesAsync(cancellationToken);
      }

      return _tokenService.Generate(user, verified);
    }
  }
}

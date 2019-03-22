using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Infrastructure.Facebook;
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

    public SignInWithFacebookCommandHandler(
      SkelvyContext context,
      IFacebookService facebookService,
      ITokenService tokenService)
    {
      _context = context;
      _facebookService = facebookService;
      _tokenService = tokenService;
    }

    public async Task<string> Handle(SignInWithFacebookCommand request, CancellationToken cancellationToken)
    {
      var verified = await _facebookService.Verify(request.AuthToken);

      var user = await _context.Users.FirstOrDefaultAsync(x => x.FacebookId == verified.UserId, cancellationToken);

      if (user == null)
      {
        var details = await _facebookService.GetBody<dynamic>(
          "me",
          request.AuthToken,
          "fields=birthday,email,first_name,gender,picture.width(512).height(512){url}");

        var email = (string)details.email;
        var userByEmail = await _context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (userByEmail == null)
        {
          user = new User
          {
            Email = details.email,
            FacebookId = verified.UserId
          };
          _context.Users.Add(user);

          var profile = new UserProfile
          {
            Name = details.first_name,
            Birthday = DateTimeOffset.ParseExact(
              (string)details.birthday,
              "MM/dd/yyyy",
              CultureInfo.CurrentCulture),
            Gender = details.gender == GenderTypes.Female ? GenderTypes.Female : GenderTypes.Male,
            UserId = user.Id
          };

          _context.UserProfiles.Add(profile);

          if (details.picture != null)
          {
            var photo = new UserProfilePhoto
            {
              Url = details.picture.data.url,
              ProfileId = profile.Id
            };

            _context.UserProfilePhotos.Add(photo);
          }
        }
        else
        {
          userByEmail.FacebookId = verified.UserId;
          user = userByEmail;
        }

        await _context.SaveChangesAsync(cancellationToken);
      }

      return _tokenService.Generate(user, verified);
    }
  }
}

using System;
using System.Globalization;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Skelvy.Application.Auth.Events.UserCreated;
using Skelvy.Application.Auth.Infrastructure.Google;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Auth.Commands.SignInWithGoogle
{
  public class SignInWithGoogleCommandHandler : QueryHandler<SignInWithGoogleCommand, AuthDto>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IProfilesRepository _profilesRepository;
    private readonly IGoogleService _googleService;
    private readonly ITokenService _tokenService;
    private readonly IMediator _mediator;
    private readonly ILogger<SignInWithGoogleCommandHandler> _logger;

    public SignInWithGoogleCommandHandler(
      IUsersRepository usersRepository,
      IProfilesRepository profilesRepository,
      IGoogleService googleService,
      ITokenService tokenService,
      IMediator mediator,
      ILogger<SignInWithGoogleCommandHandler> logger)
    {
      _usersRepository = usersRepository;
      _profilesRepository = profilesRepository;
      _googleService = googleService;
      _tokenService = tokenService;
      _mediator = mediator;
      _logger = logger;
    }

    public override async Task<AuthDto> Handle(SignInWithGoogleCommand request)
    {
      var verified = await _googleService.Verify(request.AuthToken);
      var accountCreated = false;
      var user = await _usersRepository.FindOneWithRolesByGoogleId(verified.UserId);

      if (user == null)
      {
        var details = await _googleService.GetBody<dynamic>(
          "plus/v1/people/me",
          request.AuthToken,
          "fields=birthday,name/givenName,emails/value,gender");

        var email = (string)details.emails[0].value;

        if (email == null)
        {
          user = new User($"{(string)details.name.givenName}.{verified.UserId}", request.Language);
          user.RegisterGoogle(verified.UserId);
          await CreateUserWithProfile(user, details);

          accountCreated = true;
        }
        else
        {
          var userByEmail = await _usersRepository.FindOneWithRolesByEmail(email);

          if (userByEmail == null)
          {
            if (email.Length > 50)
            {
              throw new UnauthorizedException($"{nameof(User)}(GoogleId = {verified.UserId}, Email = {email}) has too long email.");
            }

            user = new User(email, $"{(string)details.name.givenName}.{verified.UserId}", request.Language);
            user.RegisterGoogle(verified.UserId);
            await CreateUserWithProfile(user, details);

            if (user.Email != null)
            {
              await _mediator.Publish(new UserCreatedEvent(user.Id, user.Email, user.Language));
            }

            accountCreated = true;
          }
          else
          {
            ValidateUser(userByEmail);
            userByEmail.RegisterGoogle(verified.UserId);
            await _usersRepository.Update(userByEmail);

            user = userByEmail;
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
            "yyyy-MM-dd",
            CultureInfo.CurrentCulture).ToUniversalTime()
          : DateTimeOffset.UtcNow;

        var profile = new Profile(
          (string)details.name.givenName,
          birthday <= DateTimeOffset.UtcNow.AddYears(-18) ? birthday : DateTimeOffset.UtcNow.AddYears(-18),
          details.gender == GenderType.Female ? GenderType.Female : GenderType.Male,
          user.Id);

        await _profilesRepository.Add(profile);

        transaction.Commit();
      }
    }

    private static void ValidateUser(User user)
    {
      if (user.IsRemoved)
      {
        throw new UnauthorizedException($"{nameof(User)}({user.Id}) is in safety retention window for deletion.");
      }

      if (user.IsDisabled)
      {
        throw new UnauthorizedException($"{nameof(User)}({user.Id}) has been disabled.");
      }
    }
  }
}

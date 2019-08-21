using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Relations.Commands.InviteFriend;
using Skelvy.Application.Relations.Commands.InviteFriendResponse;
using Skelvy.Application.Relations.Commands.RemoveFriend;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Relations.Queries.FindFriendInvitations;
using Skelvy.Application.Relations.Queries.FindFriends;
using Skelvy.Domain.Enums;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Relations.Integration
{
  [Obsolete("Integration test should be done at API level")]
  public class FriendsFlowIntegrationTest : DatabaseRequestTestBase
  {
    private const int UserOneId = 1;
    private const int UserTwoId = 2;

    private readonly RelationsRepository _relationsRepository;
    private readonly IFriendInvitationsRepository _friendInvitationsRepository;
    private readonly UsersRepository _usersRepository;
    private readonly Mock<IMediator> _mediator;

    public FriendsFlowIntegrationTest()
    {
      var context = TestDbContext();
      _relationsRepository = new RelationsRepository(context);
      _friendInvitationsRepository = new FriendInvitationsRepository(context);
      _usersRepository = new UsersRepository(context);
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task Test()
    {
      await UserOneSendsInviteToUserTwo();
      Assert.True(await UserTwoShouldHaveInviteFromUserOne());
      await UserTwoAcceptsInviteFromUserOne();
      Assert.True(await InviteRemovedAfterAccepting());
      Assert.True(await UserOneAndUserTwoHaveAFriendsRelation());
      Assert.True(await UserOneAndUserTwoShouldHaveThemselvesOnFriendsList());
      await UserOneRemovesUserTwoFromFriendsList();
      Assert.True(await UserOneAndUserTwoShouldHaveNoRelation());
      Assert.True(await UsersOneAndTwoShouldNotHaveThemselvesOnFriendsList());
    }

    private static SkelvyContext TestDbContext()
    {
      var context = DbContext();
      SkelvyInitializer.SeedUsers(context);
      SkelvyInitializer.SeedProfiles(context);
      return context;
    }

    private async Task UserOneSendsInviteToUserTwo()
    {
      var command = new InviteFriendCommand(UserOneId, UserTwoId);
      var handler =
        new InviteFriendCommandHandler(_relationsRepository, _friendInvitationsRepository, _usersRepository, _mediator.Object);

      await handler.Handle(command);
    }

    private async Task<bool> UserTwoShouldHaveInviteFromUserOne()
    {
      var query = new FindFriendInvitationsQuery(UserTwoId);
      var handler =
        new FindFriendInvitationsQueryHandler(_friendInvitationsRepository, _usersRepository, Mapper());

      var invites = await handler.Handle(query);

      return invites != null && invites.FirstOrDefault().InvitingUser.Id == UserOneId;
    }

    private async Task UserTwoAcceptsInviteFromUserOne()
    {
      var invitation = _friendInvitationsRepository.FindAllWithInvitingDetailsByUserId(UserTwoId).Result.FirstOrDefault();

      var userFriendsRequestResponseCommand =
        new InviteFriendResponseCommand(UserTwoId, invitation.Id, true);
      var userFriendsRequestResponseCommandHandler =
        new InviteFriendResponseCommandHandler(_relationsRepository, _friendInvitationsRepository, _usersRepository, _mediator.Object);

      await userFriendsRequestResponseCommandHandler.Handle(userFriendsRequestResponseCommand);
    }

    private async Task<bool> InviteRemovedAfterAccepting()
    {
      var query = new FindFriendInvitationsQuery(UserTwoId);
      var handler =
        new FindFriendInvitationsQueryHandler(_friendInvitationsRepository, _usersRepository, Mapper());

      var invites = await handler.Handle(query);
      var invitesInDatabase = await _friendInvitationsRepository.FindAllWithInvitingDetailsByUserId(UserTwoId);
      return !invites.Any() && !invitesInDatabase.Any();
    }

    private async Task<bool> UserOneAndUserTwoHaveAFriendsRelation()
    {
      var relations = await _relationsRepository.FindAllByUserIdAndRelatedUserIdAndTypeTwoWay(UserOneId, UserTwoId, RelationType.Friend);

      return relations.Count == 2 && relations.All(r => r.Type == RelationType.Friend);
    }

    private async Task<bool> UserOneAndUserTwoShouldHaveThemselvesOnFriendsList()
    {
      var getUserFriendsListQueryUserOne = new FindFriendsQuery(UserOneId, 1);
      var getUserFriendsListQueryUserTwo = new FindFriendsQuery(UserTwoId, 1);

      var getUserFriendsListQueryHandler =
        new FindFriendsQueryHandler(_relationsRepository, _usersRepository, Mapper());

      var userOneFriendsList = await getUserFriendsListQueryHandler.Handle(getUserFriendsListQueryUserOne);
      var userTwoFriendsList = await getUserFriendsListQueryHandler.Handle(getUserFriendsListQueryUserTwo);

      return userOneFriendsList.Count == 1 && userOneFriendsList.First().Id == 2 &&
             userTwoFriendsList.Count == 1 && userTwoFriendsList.First().Id == 1;
    }

    private async Task UserOneRemovesUserTwoFromFriendsList()
    {
      var command =
        new RemoveFriendCommand(UserOneId, UserTwoId);
      var handler =
        new RemoveFriendCommandHandler(_relationsRepository, _usersRepository);

      await handler.Handle(command);
    }

    private async Task<bool> UsersOneAndTwoShouldNotHaveThemselvesOnFriendsList()
    {
      var getUserFriendsListQueryUserOne = new FindFriendsQuery(UserOneId, 1);
      var getUserFriendsListQueryUserTwo = new FindFriendsQuery(UserTwoId, 1);

      var handler =
        new FindFriendsQueryHandler(_relationsRepository, _usersRepository, Mapper());

      var userOneFriendsList = await handler.Handle(getUserFriendsListQueryUserOne);
      var userTwoFriendsList = await handler.Handle(getUserFriendsListQueryUserTwo);

      return userOneFriendsList.Count == 0 && userTwoFriendsList.Count == 0;
    }

    private async Task<bool> UserOneAndUserTwoShouldHaveNoRelation()
    {
      var relationExists = await _relationsRepository.ExistsOneByUserIdAndRelatedUserIdAndTypeTwoWay(UserOneId, UserTwoId, RelationType.Friend);
      return !relationExists;
    }
  }
}

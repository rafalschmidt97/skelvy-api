using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Relations.Queries.FindFriendInvitations
{
  public class FindFriendInvitationsQueryHandler : QueryHandler<FindFriendInvitationsQuery, IList<FriendInvitationsDto>>
  {
    private readonly IFriendInvitationsRepository _friendInvitationsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public FindFriendInvitationsQueryHandler(IFriendInvitationsRepository friendInvitationsRepository, IUsersRepository usersRepository, IMapper mapper)
    {
      _friendInvitationsRepository = friendInvitationsRepository;
      _usersRepository = usersRepository;
      _mapper = mapper;
    }

    public override async Task<IList<FriendInvitationsDto>> Handle(FindFriendInvitationsQuery invitation)
    {
      var userExists = await _usersRepository.ExistsOne(invitation.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), invitation.UserId);
      }

      var invitations = await _friendInvitationsRepository.FindAllWithInvitingDetailsByUserId(invitation.UserId);
      return _mapper.Map<IList<FriendInvitationsDto>>(invitations);
    }
  }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Relations.Infrastructure;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class RelationsRepository : BaseRepository, IRelationsRepository
  {
    public RelationsRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<Relation>> FindAllRelationsByUserIdAndRelatedUserIdAndType(int userId, int relatedUserId, string type)
    {
      return await Context.Relations.Where(x =>
        (x.UserId == userId && x.RelatedUserId == relatedUserId && !x.IsRemoved && x.Type == type) ||
        (x.UserId == relatedUserId && x.RelatedUserId == userId && !x.IsRemoved && x.Type == type)).ToListAsync();
    }

    public async Task<IList<Relation>> FindPageRelationsUsersWithRelatedDetailsByUserIdAndType(int userId, string relationType, int page, int pageSize = 10)
    {
      var skip = (page - 1) * pageSize;
      var relations = await Context.Relations
        .Include(x => x.RelatedUser)
        .ThenInclude(x => x.Profile)
        .Where(x => x.UserId == userId && x.Type == relationType && !x.IsRemoved)
        .OrderBy(x => x.Id)
        .Skip(skip)
        .Take(pageSize)
        .ToListAsync();

      foreach (var relation in relations)
      {
        var userPhotos = await Context.UserProfilePhotos
          .Include(x => x.Attachment)
          .Where(x => x.ProfileId == relation.RelatedUser.Profile.Id)
          .OrderBy(x => x.Order)
          .ToListAsync();

        relation.RelatedUser.Profile.Photos = userPhotos;
      }

      return relations;
    }

    public async Task<IList<FriendRequest>> FindAllFriendRequestsWithInvitingDetailsByUserId(int userId)
    {
      var requests = await Context.FriendRequests
        .Include(x => x.InvitingUser)
        .ThenInclude(x => x.Profile)
        .Where(r => r.InvitedUserId == userId && !r.IsRemoved)
        .ToListAsync();

      foreach (var request in requests)
      {
        request.InvitingUser.Profile.Photos = await Context.UserProfilePhotos
          .Where(x => x.ProfileId == request.InvitingUser.Profile.Id)
          .OrderBy(x => x.Order)
          .ToListAsync();
      }

      return requests;
    }

    public async Task<FriendRequest> FindOneFriendRequestByRequestId(int requestId)
    {
      return await Context.FriendRequests
        .FirstOrDefaultAsync(r => r.Id == requestId && !r.IsRemoved);
    }

    public async Task<bool> ExistsFriendRequestByInvitingIdAndInvitedId(int invitingUserId, int invitedUserId)
    {
      return await Context.FriendRequests.AnyAsync(
        x => (x.InvitingUserId == invitingUserId || x.InvitedUserId == invitedUserId) && !x.IsRemoved);
    }

    public async Task<bool> ExistsRelationByUserIdAndRelatedUserIdAndType(int userId, int relatedUserId, string type)
    {
      return await Context.Relations.AnyAsync(
        x => ((x.UserId == userId && x.RelatedUserId == relatedUserId) || (x.UserId == relatedUserId && x.RelatedUserId == userId)) &&
             !x.IsRemoved &&
             x.Type == type);
    }

    public async Task<IList<Relation>> FindAllRelationsWithRemovedByUsersId(List<int> usersId)
    {
      return await Context.Relations
        .Where(x => usersId.Any(y => y == x.UserId || y == x.RelatedUserId))
        .ToListAsync();
    }

    public async Task<IList<FriendRequest>> FindAllFriendRequestWithRemovedByUsersId(List<int> usersId)
    {
      return await Context.FriendRequests
        .Where(x => usersId.Any(y => y == x.InvitedUserId || y == x.InvitingUserId))
        .ToListAsync();
    }

    public async Task AddRangeRelations(IList<Relation> relations)
    {
      await Context.Relations.AddRangeAsync(relations);
      await Context.SaveChangesAsync();
    }

    public async Task AddFriendRequest(FriendRequest friendsRequest)
    {
      await Context.FriendRequests.AddAsync(friendsRequest);
      await Context.SaveChangesAsync();
    }

    public async Task UpdateFriendsRequest(FriendRequest request)
    {
      Context.FriendRequests.Update(request);
      await Context.SaveChangesAsync();
    }

    public async Task UpdateRelation(Relation relation)
    {
      Context.Relations.Update(relation);
      await Context.SaveChangesAsync();
    }

    public async Task UpdateRangeRelation(IList<Relation> relations)
    {
      Context.Relations.UpdateRange(relations);
      await Context.SaveChangesAsync();
    }

    public async Task RemoveRangeRelation(IList<Relation> relations)
    {
      Context.Relations.RemoveRange(relations);
      await Context.SaveChangesAsync();
    }

    public async Task RemoveRangeFriendRequest(IList<FriendRequest> friendRequests)
    {
      Context.FriendRequests.RemoveRange(friendRequests);
      await Context.SaveChangesAsync();
    }
  }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

    public async Task<Relation> FindOneByUserIdAndRelatedUserIdAndType(int userId, int relatedUserId, string type)
    {
      return await Context.Relations.FirstOrDefaultAsync(x =>
        x.UserId == userId && x.RelatedUserId == relatedUserId && x.Type == type && !x.IsRemoved);
    }

    public async Task<Relation> FindOneByUserIdAndRelatedUserIdTwoWay(int userId, int relatedUserId)
    {
      return await Context.Relations.Where(x =>
        (x.UserId == userId && x.RelatedUserId == relatedUserId && !x.IsRemoved) ||
        (x.UserId == relatedUserId && x.RelatedUserId == userId && !x.IsRemoved)).FirstOrDefaultAsync();
    }

    public async Task<IList<Relation>> FindAllByUserIdAndRelatedUserIdAndTypeTwoWay(int userId, int relatedUserId, string type)
    {
      return await Context.Relations.Where(x =>
        (x.UserId == userId && x.RelatedUserId == relatedUserId && !x.IsRemoved && x.Type == type) ||
        (x.UserId == relatedUserId && x.RelatedUserId == userId && !x.IsRemoved && x.Type == type)).ToListAsync();
    }

    public async Task<IList<Relation>> FindPageUsersWithRelatedDetailsByUserIdAndType(int userId, string relationType, int page, int pageSize = 10)
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
        var userPhotos = await Context.ProfilePhotos
          .Include(x => x.Attachment)
          .Where(x => x.ProfileId == relation.RelatedUser.Profile.Id)
          .OrderBy(x => x.Order)
          .ToListAsync();

        relation.RelatedUser.Profile.Photos = userPhotos;
      }

      return relations;
    }

    public async Task<bool> ExistsOneByUserIdAndRelatedUserIdAndTypeTwoWay(int userId, int relatedUserId, string type)
    {
      return await Context.Relations.AnyAsync(
        x => ((x.UserId == userId && x.RelatedUserId == relatedUserId) ||
              (x.UserId == relatedUserId && x.RelatedUserId == userId)) &&
             x.Type == type &&
             !x.IsRemoved);
    }

    public async Task<bool> ExistsOneByUserIdAndRelatedUserIdAndType(int userId, int relatedUserId, string type)
    {
      return await Context.Relations.AnyAsync(
        x => x.UserId == userId && x.RelatedUserId == relatedUserId && x.Type == type && !x.IsRemoved);
    }

    public async Task<IList<Relation>> FindAllWithRemovedByUsersId(List<int> usersId)
    {
      return await Context.Relations
        .Where(x => usersId.Any(y => y == x.UserId || y == x.RelatedUserId))
        .ToListAsync();
    }

    public async Task Add(Relation relation)
    {
      await Context.Relations.AddAsync(relation);
      await Context.SaveChangesAsync();
    }

    public async Task AddRange(IList<Relation> relations)
    {
      await Context.Relations.AddRangeAsync(relations);
      await Context.SaveChangesAsync();
    }

    public async Task Update(Relation relation)
    {
      Context.Relations.Update(relation);
      await Context.SaveChangesAsync();
    }

    public async Task UpdateRange(IList<Relation> relations)
    {
      Context.Relations.UpdateRange(relations);
      await Context.SaveChangesAsync();
    }

    public async Task RemoveRange(IList<Relation> relations)
    {
      Context.Relations.RemoveRange(relations);
      await Context.SaveChangesAsync();
    }
  }
}

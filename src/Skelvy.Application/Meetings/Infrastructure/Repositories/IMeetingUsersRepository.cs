using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Infrastructure.Repositories
{
  public interface IMeetingUsersRepository : IBaseRepository
  {
    Task<MeetingUser> FindOneByUserId(int userId);
  }
}

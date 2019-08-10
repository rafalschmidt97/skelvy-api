using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Activities.Infrastructure.Repositories
{
  public interface IActivitiesRepository : IBaseRepository
  {
    Task<IList<Activity>> FindAll();
  }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Activities.Infrastructure.Repositories;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Activities.Queries.FindActivities
{
  public class FindActivitiesQueryHandler : QueryHandler<FindActivitiesQuery, IList<ActivityDto>>
  {
    private readonly IActivitiesRepository _repository;
    private readonly IMapper _mapper;

    public FindActivitiesQueryHandler(IActivitiesRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override async Task<IList<ActivityDto>> Handle(FindActivitiesQuery request)
    {
      var activities = await _repository.FindAll();
      return _mapper.Map<IList<ActivityDto>>(activities);
    }
  }
}

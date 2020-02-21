using System.Collections.Generic;
using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Activities.Queries.FindActivities
{
  public class FindActivitiesQuery : IQuery<IList<ActivityDto>>
  {
    public FindActivitiesQuery(bool restricted)
    {
      Restricted = restricted;
    }

    [JsonConstructor]
    public FindActivitiesQuery()
    {
    }

    public bool Restricted { get; set; }
  }
}

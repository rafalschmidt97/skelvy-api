using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Activities.Queries.FindActivities
{
  public class FindActivitiesQuery : IQuery<IList<ActivityDto>>
  {
    public FindActivitiesQuery(bool restricted)
    {
      Restricted = restricted;
    }

    public FindActivitiesQuery() // required for FromQuery attribute
    {
    }

    public bool Restricted { get; set; }
  }
}

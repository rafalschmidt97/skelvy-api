using System.Collections.Generic;
using MediatR;
using Skelvy.Application.Infrastructure.Maps;

namespace Skelvy.Application.Maps.Queries.ReverseLocations
{
  public class ReverseLocationsQuery : IRequest<ICollection<Location>>
  {
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Language { get; set; }
  }
}

using System.Reflection;
using AutoMapper;

namespace Skelvy.Application.Core.Mappers
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      LoadCustomMappings();
    }

    private void LoadCustomMappings()
    {
      var mapsFrom = MappingProfileHelper.LoadCustomMappings(Assembly.GetExecutingAssembly());

      foreach (var map in mapsFrom)
      {
        map.CreateMappings(this);
      }
    }
  }
}

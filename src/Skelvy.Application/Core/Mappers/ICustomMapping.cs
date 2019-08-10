using AutoMapperProfile = AutoMapper.Profile;

namespace Skelvy.Application.Core.Mappers
{
  public interface ICustomMapping
  {
    void CreateMappings(AutoMapperProfile configuration);
  }
}

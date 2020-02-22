using AutoMapperProfile = AutoMapper.Profile;

namespace Skelvy.Application.Core.Mappers
{
  public interface IMapping<T>
  {
    void Mapping(AutoMapperProfile profile) =>
      profile.CreateMap(typeof(T), GetType());
  }
}

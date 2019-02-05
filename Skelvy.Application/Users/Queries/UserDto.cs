namespace Skelvy.Application.Users.Queries
{
  public class UserDto
  {
    public int Id { get; set; }

    public string Name { get; set; }

    // public void CreateMappings(Profile configuration)
    // {
    //   configuration.CreateMap<User, UserDto>()
    //     .ForMember(
    //       to => to.UserEmail,
    //       from => from.MapFrom(p => $"{p.Name} - {p.Email}"));
    // }
  }
}

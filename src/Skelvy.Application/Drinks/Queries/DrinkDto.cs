namespace Skelvy.Application.Drinks.Queries
{
  public class DrinkDto
  {
    public DrinkDto(int id, string name)
    {
      Id = id;
      Name = name;
    }

    public int Id { get; }
    public string Name { get; }
  }
}

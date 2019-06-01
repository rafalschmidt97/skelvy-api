namespace Skelvy.Domain.Entities
{
  public class DrinkType
  {
    public DrinkType(string name)
    {
      Name = name;
    }

    public DrinkType(int id, string name)
    {
      Id = id;
      Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
  }
}

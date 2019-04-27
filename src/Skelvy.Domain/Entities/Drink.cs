namespace Skelvy.Domain.Entities
{
  public class Drink
  {
    public Drink(string name)
    {
      Name = name;
    }

    public Drink(int id, string name)
    {
      Id = id;
      Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
  }
}

namespace Skelvy.Domain.Entities
{
  public class DrinkType
  {
    public DrinkType(string name)
    {
      Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; }
  }
}

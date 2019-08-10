namespace Skelvy.Domain.Entities
{
  public class Activity
  {
    public Activity(string name)
    {
      Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; }
  }
}

namespace Skelvy.Domain.Entities
{
  public class Activity
  {
    public Activity(string name, string type, int size, int distance)
    {
      Name = name;
      Type = type;
      Size = size;
      Distance = distance;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int Size { get; set; }
    public int Distance { get; set; }
  }
}

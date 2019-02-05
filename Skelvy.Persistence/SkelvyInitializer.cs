namespace Skelvy.Persistence
{
  public static class SkelvyInitializer
  {
    public static void Initialize(SkelvyContext context)
    {
      SeedEverything(context);
    }

    private static void SeedEverything(SkelvyContext context)
    {
      context.Database.EnsureCreated();
    }
  }
}

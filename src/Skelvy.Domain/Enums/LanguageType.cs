namespace Skelvy.Domain.Enums
{
  public static class LanguageType
  {
    public const string PL = "pl";
    public const string EN = "en";
    public const string DE = "de";
    public const string ES = "es";

    public static bool Check(string language)
    {
      return language == EN ||
             language == PL ||
             language == DE ||
             language == ES;
    }

    public static T Switch<T>(string language, T en, T pl, T de, T es)
    {
      switch (language)
      {
        case EN: return en;
        case PL: return pl;
        case DE: return de;
        case ES: return es;
        default: return en;
      }
    }

    public static string CheckFailedResponse()
    {
      return $"'Language' must be {PL} / {EN} / {DE} / {ES}";
    }
  }
}

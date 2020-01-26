namespace Skelvy.Domain.Enums
{
  public static class LanguageType
  {
    public const string PL = "pl";
    public const string EN = "en";
    public const string DE = "de";
    public const string ES = "es";
    public const string FR = "fr";
    public const string IT = "it";
    public const string RU = "ru";
    public const string FI = "fi";

    public static bool Check(string language)
    {
      return language == EN ||
             language == PL ||
             language == DE ||
             language == ES ||
             language == FR ||
             language == IT ||
             language == RU ||
             language == FI;
    }

    public static T Switch<T>(string language, T en, T pl, T de, T es, T fr, T it, T ru, T fi)
    {
      switch (language)
      {
        case EN: return en;
        case PL: return pl;
        case DE: return de;
        case ES: return es;
        case FR: return fr;
        case IT: return it;
        case RU: return ru;
        case FI: return fi;
        default: return en;
      }
    }

    public static string CheckFailedResponse()
    {
      return $"'Language' must be {PL} / {EN} / {DE} / {ES} / {FR} / {IT} / {RU} / {FI}";
    }
  }
}

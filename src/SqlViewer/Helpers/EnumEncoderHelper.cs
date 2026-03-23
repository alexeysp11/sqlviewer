using SqlViewer.Enums.Localization;

namespace SqlViewer.Helpers;

public static class EnumEncoderHelper
{
    public static LanguageEnum GetLanguageEnum(string language) => language switch
    {
        "English" => LanguageEnum.English,
        "German" => LanguageEnum.German,
        "Russian" => LanguageEnum.Russian,
        "Spanish" => LanguageEnum.Spanish,
        "Portuguese" => LanguageEnum.Portuguese,
        "Italian" => LanguageEnum.Italian,
        "French" => LanguageEnum.French,
        "Ukranian" => LanguageEnum.Ukranian,
        "Dutch" => LanguageEnum.Dutch,
        "Polish" => LanguageEnum.Polish,
        "Czech" => LanguageEnum.Czech,
        "Serbian" => LanguageEnum.Serbian,
        "Croatian" => LanguageEnum.Croatian,
        "Swedish" => LanguageEnum.Swedish,
        "Norwegian" => LanguageEnum.Norwegian,
        "Danish" => LanguageEnum.Danish,
        "Afrikaans" => LanguageEnum.Afrikaans,
        "Turkish" => LanguageEnum.Turkish,
        "Kazakh" => LanguageEnum.Kazakh,
        "Armenian" => LanguageEnum.Armenian,
        "Georgian" => LanguageEnum.Georgian,
        "Romanian" => LanguageEnum.Romanian,
        "Bulgarian" => LanguageEnum.Bulgarian,
        "Albanian" => LanguageEnum.Albanian,
        "Greek" => LanguageEnum.Greek,
        "Indonesian" => LanguageEnum.Indonesian,
        "Malay" => LanguageEnum.Malay,
        "Korean" => LanguageEnum.Korean,
        "Japanese" => LanguageEnum.Japanese,
        _ => throw new Exception("Unable to find language enum")
    };
}

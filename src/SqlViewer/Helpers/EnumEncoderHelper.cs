using SqlViewer.Enums.Editor;
using SqlViewer.Enums.Localization;
using VelocipedeUtils.Shared.DbOperations.Enums;

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

    public static AutoSaveEnum GetAutoSaveEnum(string autoSave) => autoSave switch
    {
        "Enabled" => AutoSaveEnum.Enabled,
        "Disabled" => AutoSaveEnum.Disabled,
        _ => throw new Exception("Unable to find auto-save enum")
    };

    public static FontSizeEnum GetFontSizeEnum(string fontSize) => fontSize switch
    {
        "8" => FontSizeEnum.FontSize8,
        "9" => FontSizeEnum.FontSize9,
        "10" => FontSizeEnum.FontSize10,
        "11" => FontSizeEnum.FontSize11,
        "12" => FontSizeEnum.FontSize12,
        "14" => FontSizeEnum.FontSize14,
        "16" => FontSizeEnum.FontSize16,
        "18" => FontSizeEnum.FontSize18,
        "20" => FontSizeEnum.FontSize20,
        _ => throw new Exception("Unable to find font size enum")
    };

    public static FontFamilyEnum GetFontFamilyEnum(string fontFamily) => fontFamily switch
    {
        "Consolas" => FontFamilyEnum.Consolas,
        _ => throw new Exception("Unable to find font family enum")
    };

    public static TabSizeEnum GetTabSizeEnum(string tabSize) => tabSize switch
    {
        "1" => TabSizeEnum.TabSize1,
        "2" => TabSizeEnum.TabSize2,
        "3" => TabSizeEnum.TabSize3,
        "4" => TabSizeEnum.TabSize4,
        "5" => TabSizeEnum.TabSize5,
        "6" => TabSizeEnum.TabSize6,
        "7" => TabSizeEnum.TabSize7,
        "8" => TabSizeEnum.TabSize8,
        _ => throw new Exception("Unable to find tab size enum")
    };

    public static WordWrapEnum GetWordWrapEnum(string wordWrap) => wordWrap switch
    {
        "Enabled" => WordWrapEnum.Enabled,
        "Disabled" => WordWrapEnum.Disabled,
        _ => throw new Exception("Unable to find word wrap enum")
    };

    public static VelocipedeDatabaseType GetRdbmsEnum(string rdbms) => rdbms switch
    {
        "SQLite" => VelocipedeDatabaseType.SQLite,
        "PostgreSQL" => VelocipedeDatabaseType.PostgreSQL,
        "SQL Server" => VelocipedeDatabaseType.MSSQL,
        "MySQL" => VelocipedeDatabaseType.MySQL,
        "Oracle" => VelocipedeDatabaseType.Oracle,
        _ => throw new Exception("Unable to find RDBMS enum")
    };
}

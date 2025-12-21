using SqlViewer.Models.EnumOperations;
using LanguageEnum = SqlViewer.Enums.Common.Language;
using AutoSaveEnum = SqlViewer.Enums.Editor.AutoSave;
using FontSizeEnum = SqlViewer.Enums.Editor.FontSize;
using FontFamilyEnum = SqlViewer.Enums.Editor.FontFamily;
using TabSizeEnum = SqlViewer.Enums.Editor.TabSize;
using WordWrapEnum = SqlViewer.Enums.Editor.WordWrap;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Models.DataStorage;

public class AppSettingsRepo
{
    private EnumEncoder EnumEncoder { get; set; }

    public LanguageEnum Language { get; private set; }
    public AutoSaveEnum AutoSave { get; private set; }
    public FontSizeEnum FontSize { get; private set; }
    public FontFamilyEnum FontFamily { get; private set; }
    public TabSizeEnum TabSize { get; private set; }
    public WordWrapEnum WordWrap { get; private set; }

    public VelocipedeDatabaseType DefaultRdbms { get; private set; }
    public VelocipedeDatabaseType ActiveRdbms { get; private set; }

    public string DbHost { get; private set; }
    public string DbPort { get; private set; }
    public string DbName { get; private set; }
    public string DbSchema { get; private set; }
    public string DbUsername { get; private set; }
    public string DbPassword { get; private set; }

    public AppSettingsRepo(EnumEncoder enumEncoder, string language, string autoSave, int fontSize, string fontFamily,
        int tabSize, string wordWrap, string defaultRdbms, string activeRdbms)
    {
        EnumEncoder = enumEncoder;
        AssignBasic(language, autoSave, fontSize, fontFamily, tabSize, wordWrap, defaultRdbms, activeRdbms);
    }

    public AppSettingsRepo(EnumEncoder enumEncoder, string language, string autoSave, int fontSize, string fontFamily,
        int tabSize, string wordWrap, string defaultRdbms, string activeRdbms, string server, string dbName, string port,
        string dbSchema, string dbUsername, string dbPassword)
    {
        EnumEncoder = enumEncoder;
        AssignBasic(language, autoSave, fontSize, fontFamily, tabSize, wordWrap, defaultRdbms, activeRdbms);
        AssignDbCredentials(server, dbName, port, dbSchema, dbUsername, dbPassword);
    }

    private void AssignBasic(string language, string autoSave, int fontSize, string fontFamily,
        int tabSize, string wordWrap, string defaultRdbms, string activeRdbms)
    {
        Language = EnumEncoder.GetLanguageEnum($"{language}");
        AutoSave = EnumEncoder.GetAutoSaveEnum($"{autoSave}");
        FontSize = EnumEncoder.GetFontSizeEnum($"{fontSize}");
        FontFamily = EnumEncoder.GetFontFamilyEnum($"{fontFamily}");
        TabSize = EnumEncoder.GetTabSizeEnum($"{tabSize}");
        WordWrap = EnumEncoder.GetWordWrapEnum($"{wordWrap}");
        DefaultRdbms = EnumEncoder.GetRdbmsEnum($"{defaultRdbms}");
        ActiveRdbms = EnumEncoder.GetRdbmsEnum($"{activeRdbms}");
    }

    private void AssignDbCredentials(string server, string dbName, string port, string dbSchema, string dbUsername, string dbPassword)
    {
        DbHost = server;
        DbName = dbName;
        DbPort = port;
        DbSchema = dbSchema;
        DbUsername = dbUsername;
        DbPassword = dbPassword;
    }

    public void Update(string language, string autoSave, int fontSize, string fontFamily, int tabSize,
        string wordWrap, string defaultRdbms, string activeRdbms, string server, string dbName, string port,
        string dbSchema, string dbUsername, string dbPassword)
    {
        AssignBasic(language, autoSave, fontSize, fontFamily, tabSize, wordWrap, defaultRdbms, activeRdbms);
        AssignDbCredentials(server, dbName, port, dbSchema, dbUsername, dbPassword);
    }

    public void SetActiveRdbms(string activeRdbms)
    {
        if (string.IsNullOrEmpty(activeRdbms))
        {
            throw new Exception("Parameter 'activeRdbms' should not be null or empty");
        }
        ActiveRdbms = EnumEncoder.GetRdbmsEnum($"{activeRdbms}");
    }

    public void SetDbName(string dbName)
    {
        if (string.IsNullOrEmpty(dbName))
        {
            throw new Exception("Parameter 'dbName' should not be null or empty");
        }
        DbName = dbName;
    }
}

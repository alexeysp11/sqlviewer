using RdbmsEnum = SqlViewer.Enums.Database.Rdbms;

namespace SqlViewer.Helpers;

/// <summary>
/// Class for application settings 
/// </summary>
public static class SettingsHelper
{
    public static string FilterFileSystemDb => "All files|*.*|Database files|*.db|SQLite3 files|*.sqlite3";

    public static string RootFolder => AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\..";

    public static string ExtensionsFolder => RootFolder + "\\extension";

    public static string GetActiveRdbmsString()
    {
        return string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.ActiveRdbms.ToString()) ? RdbmsEnum.SQLite.ToString() : RepoHelper.AppSettingsRepo.ActiveRdbms.ToString();
    }

    public static string GetTmpTableTransfer()
    {
        DateTime now = DateTime.UtcNow;

        string year = now.Year.ToString();
        string month = (now.Month < 10 ? "0" : "") + now.Month.ToString();
        string day = (now.Day < 10 ? "0" : "") + now.Day.ToString();
        string hour = (now.Hour < 10 ? "0" : "") + now.Hour.ToString();
        string minute = (now.Minute < 10 ? "0" : "") + now.Minute.ToString();
        string second = (now.Second < 10 ? "0" : "") + now.Second.ToString();

        return "tmp" + year + month + day + hour + minute + second;
    }
}

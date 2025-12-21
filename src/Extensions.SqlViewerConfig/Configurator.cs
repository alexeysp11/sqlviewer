using System.IO;
using Extensions.SqlViewerConfig.DataModels;
using Microsoft.Extensions.Configuration;
using VelocipedeUtils.Shared.DbConnections;

namespace Extensions.SqlViewerConfig;

/// <summary>
/// Class for initial configuration of the project (creates all necessary databases, folders and files)
/// </summary>
public class Configurator
{
    /// <summary>
    /// Model for stroing settings 
    /// </summary>
    private Settings Settings { get; set; }

    /// <summary>
    /// Folder where the project is located 
    /// </summary>
    private string RootFolder { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets settings from config file 
    /// </summary>
    public void GetConfigSettings()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        Settings = config.GetSection("Settings").Get<Settings>();
    }

    /// <summary>
    /// Creates folders 
    /// </summary>
    public void CreateFolders()
    {
        string rootFolder = GetRootFolder();
        string[] paths = {
            Path.Combine(rootFolder, "data"),
            Path.Combine(rootFolder, "data/SqlViewerNetwork"),
            Path.Combine(rootFolder, "data/SqlViewer"),
            Path.Combine(rootFolder, "data/!backups")
        };
        foreach (string path in paths)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }

    /// <summary>
    /// Creates local databases and config files for other extensions 
    /// </summary>
    public void CreateLocalFiles()
    {
        string rootFolder = GetRootFolder();
        string[] paths = {
            Path.Combine(rootFolder, "data/app.db"),
            Path.Combine(rootFolder, "data/data.db"),
            Path.Combine(rootFolder, "data/test.db"),
            Path.Combine(rootFolder, "data/SqlViewerNetwork/settings.xml")
        };
        foreach (string path in paths)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }
    }

    /// <summary>
    /// Makes a backup of local databases 
    /// </summary>
    public void BackupData()
    {
        string rootFolder = GetRootFolder();

        string srcFile = Path.Combine(rootFolder, "data/app.db");
        string dstDir = Path.Combine(rootFolder, "data/!backups/" + GetCurrentMomentString());
        string dstFile = Path.Combine(dstDir, "app.db");

        if (!File.Exists(srcFile)) return;

        Directory.CreateDirectory(dstDir);

        File.Move(srcFile, dstFile);
    }

    /// <summary>
    /// Initializes databases 
    /// </summary>
    public void InitDatabases()
    {
        string rootFolder = GetRootFolder();
        string sqliteInitFolder = Path.Combine(rootFolder, "queries/Sqlite/Init");
        
        bool isInit = false;
        while (!isInit)
        {
            try 
            {
                SqliteDbConnection sqlite = new(Path.Combine(rootFolder, @"data\app.db"));
                sqlite.ExecuteSqlCommand(File.ReadAllText(Path.Combine(sqliteInitFolder, "InitAppDb.sql")));

                sqlite = new(Path.Combine(rootFolder, @"data\data.db"));
                sqlite.ExecuteSqlCommand(File.ReadAllText(Path.Combine(sqliteInitFolder, "InitDataDb.sql")));

                sqlite = new(Path.Combine(rootFolder, @"data\test.db"));
                sqlite.ExecuteSqlCommand(File.ReadAllText(Path.Combine(sqliteInitFolder, "InitTestDb.sql")));
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            isInit = true;
        }
    }

    /// <summary>
    /// Gets folder where the projects is located 
    /// </summary>
    private string GetRootFolder()
    {
        if (string.IsNullOrEmpty(RootFolder))
            RootFolder = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\..");
        return RootFolder;
    }

    /// <summary>
    /// Gets a string in a form '202301302201'
    /// </summary>
    private static string GetCurrentMomentString()
    {
        System.DateTime now = System.DateTime.UtcNow;

        string year = now.Year.ToString();
        string month = (now.Month < 10 ? "0" : "")  + now.Month.ToString();
        string day = (now.Day < 10 ? "0" : "") + now.Day.ToString();
        string hour = (now.Hour < 10 ? "0" : "") + now.Hour.ToString();
        string minute = (now.Minute < 10 ? "0" : "") + now.Minute.ToString();
        string second = (now.Second < 10 ? "0" : "") + now.Second.ToString();

        return year + month + day + hour + minute + second;
    }
}

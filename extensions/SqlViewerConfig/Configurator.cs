using SqlViewerDatabase.DbConnections; 
using Microsoft.Extensions.Configuration; 

namespace SqlViewerConfig
{
    public class Configurator
    {
        private Settings Settings { get; set; }

        private string RootFolder { get; set; } = string.Empty; 
        
        public void GetConfigSettings()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            Settings = config.GetSection("Settings").Get<Settings>();

            System.Console.WriteLine($"KeyOne = {Settings.KeyOne}");
            System.Console.WriteLine($"KeyTwo = {Settings.KeyTwo}");
            System.Console.WriteLine($"KeyThree:Message = {Settings.KeyThree.Message}");
        }

        public void CreateFolders()
        {
            string rootFolder = GetRootFolder(); 
            string[] paths = { 
                System.IO.Path.Combine(rootFolder, "data"), 
                System.IO.Path.Combine(rootFolder, "data/SqlViewerNetwork"), 
                System.IO.Path.Combine(rootFolder, "data/SqlViewer"), 
                System.IO.Path.Combine(rootFolder, "data/!backups") 
            }; 
            foreach (string path in paths)
            {
                if (!System.IO.Directory.Exists(path)) 
                {
                    System.IO.Directory.CreateDirectory(path); 
                }
            }
        }

        public void CreateLocalFiles()
        {
            string rootFolder = GetRootFolder(); 
            string[] paths = { 
                System.IO.Path.Combine(rootFolder, "data/app.db"), 
                System.IO.Path.Combine(rootFolder, "data/data.db"), 
                System.IO.Path.Combine(rootFolder, "data/test.db"), 
                System.IO.Path.Combine(rootFolder, "data/SqlViewerNetwork/settings.xml") 
            }; 
            foreach (string path in paths)
            {
                if (!System.IO.File.Exists(path))
                {
                    System.IO.File.Create(path); 
                }
            }
        }

        public void BackupData()
        {
            string rootFolder = GetRootFolder(); 

            string srcFile = System.IO.Path.Combine(rootFolder, "data/app.db"); 
            string dstDir = System.IO.Path.Combine(rootFolder, "data/!backups/" + GetCurrentMomentString());
            string dstFile = System.IO.Path.Combine(dstDir, "app.db"); 

            if (!System.IO.File.Exists(srcFile)) return; 
            
            System.IO.Directory.CreateDirectory(dstDir); 

            System.IO.File.Move(srcFile, dstFile); 
        }

        public void InitDatabases()
        {
            string rootFolder = GetRootFolder(); 
            string sqliteInitFolder = System.IO.Path.Combine(rootFolder, "extensions/SqlViewerConfig/Queries/Sqlite/Init"); 
            
            bool isInit = false; 
            while (!isInit)
            {
                try 
                {
                    ICommonDbConnection sqlite = new SqliteDbConnection(System.IO.Path.Combine(rootFolder, @"data\app.db")); 
                    sqlite.ExecuteSqlCommand(System.IO.File.ReadAllText(System.IO.Path.Combine(sqliteInitFolder, "InitAppDb.sql"))); 
                    sqlite.ExecuteSqlCommand(System.IO.File.ReadAllText(System.IO.Path.Combine(sqliteInitFolder, "InitAppDbSettings.sql"))); 
                    sqlite.ExecuteSqlCommand(System.IO.File.ReadAllText(System.IO.Path.Combine(sqliteInitFolder, "InitTranslation.sql"))); 

                    sqlite = new SqliteDbConnection(System.IO.Path.Combine(rootFolder, @"data\data.db")); 
                    sqlite.ExecuteSqlCommand(System.IO.File.ReadAllText(System.IO.Path.Combine(sqliteInitFolder, "InitDataDb.sql"))); 

                    sqlite = new SqliteDbConnection(System.IO.Path.Combine(rootFolder, @"data\test.db")); 
                    sqlite.ExecuteSqlCommand(System.IO.File.ReadAllText(System.IO.Path.Combine(sqliteInitFolder, "InitTestDb.sql"))); 
                }
                catch (System.Exception)
                {

                }
                isInit = true; 
            }
        }

        private string GetRootFolder()
        {
            if (string.IsNullOrEmpty(RootFolder))
                RootFolder = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\.."); 
            return RootFolder; 
        }

        private string GetCurrentMomentString()
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
}

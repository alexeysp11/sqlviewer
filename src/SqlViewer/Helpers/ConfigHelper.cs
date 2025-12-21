using System.Windows;
using SqlViewer.ViewModels;

namespace SqlViewer.Helpers;

public class ConfigHelper(MainVM mainVM, string rootFolder)
{
    private MainVM MainVM { get; set; } = mainVM;

    private string RootFolder { get; set; } = rootFolder;

    public void Initialize()
    {
        CreateDataFolder();
    }

    public void CreateDataFolder()
    {
        string dataFolderPath = $"{RootFolder}\\data";
        CreateFolderIfNotExists(dataFolderPath);

        string sqlFolderPath = $"{RootFolder}\\src\\Queries\\Sqlite\\Init";
        CreateLocalDbIfNotExists($"{dataFolderPath}\\app.db", $"{sqlFolderPath}\\InitAppDb.sql");
        CreateLocalDbIfNotExists($"{dataFolderPath}\\app.db", $"{sqlFolderPath}\\InitAppDbSettings.sql");
        CreateLocalDbIfNotExists($"{dataFolderPath}\\app.db", $"{sqlFolderPath}\\RecoverSettings.sql");
        CreateLocalDbIfNotExists($"{dataFolderPath}\\data.db", $"{sqlFolderPath}\\InitDataDb.sql");
        CreateLocalDbIfNotExists($"{dataFolderPath}\\test.db", $"{sqlFolderPath}\\InitTestDb.sql");
    }

    private static void CreateFolderIfNotExists(string folderPath)
    {
        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }
    }

    private void CreateLocalDbIfNotExists(string localDbPath, string sqlScriptPath)
    {
        if (!System.IO.File.Exists(localDbPath))
        {
            MessageBox.Show("CreateLocalDbIfNotExists");
            MainVM.DataVM.AppRdbmsPreproc.GetAppDbConnection().ExecuteSqlCommand(DataVM.GetSqlRequest(sqlScriptPath));
            MessageBox.Show("CreateLocalDbIfNotExists");
        }
    }
}

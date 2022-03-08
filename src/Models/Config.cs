using System.Windows;
using SqlViewer.ViewModels; 
using SqlViewer.Models.DbConnections; 

namespace SqlViewer.Models
{
    public class Config
    {
        private MainVM MainVM { get; set; }

        private System.String RootFolder { get; set; }

        public Config(MainVM mainVM, System.String rootFolder)
        {
            this.MainVM = mainVM; 
            this.RootFolder = rootFolder; 
        }

        public void Initialize()
        {
            CreateDataFolder(); 
            //CreateAllShortcuts(); // Not supported yet 
        }

        #region File system methods
        public void CreateDataFolder()
        {
            System.String dataFolderPath = $"{RootFolder}\\data"; 
            CreateFolderIfNotExists(dataFolderPath); 

            System.String sqlFolderPath = $"{RootFolder}\\src\\Queries\\Init"; 
            CreateLocalDbIfNotExists($"{dataFolderPath}\\app.db", $"{sqlFolderPath}\\InitAppDb.sql"); 
            CreateLocalDbIfNotExists($"{dataFolderPath}\\app.db", $"{sqlFolderPath}\\InitAppDbSettings.sql"); 
            CreateLocalDbIfNotExists($"{dataFolderPath}\\app.db", $"{sqlFolderPath}\\RecoverSettings.sql"); 
            CreateLocalDbIfNotExists($"{dataFolderPath}\\data.db", $"{sqlFolderPath}\\InitDataDb.sql"); 
            CreateLocalDbIfNotExists($"{dataFolderPath}\\test.db", $"{sqlFolderPath}\\InitTestDb.sql"); 
        }

        private void CreateFolderIfNotExists(System.String folderPath)
        {
            if ( !(System.IO.Directory.Exists(folderPath)) )
            {
                System.IO.Directory.CreateDirectory(folderPath); 
            }
        }

        private void CreateLocalDbIfNotExists(System.String localDbPath, System.String sqlScriptPath)
        {
            if ( !(System.IO.File.Exists(localDbPath)) )
            {
                System.String sql = this.MainVM.GetSqlRequest(sqlScriptPath); 
                MainVM.AppDbConnection.ExecuteSqlCommand(sql); 
            }
        }
        #endregion  // File system methods

        #region Shortcuts methods
        public void CreateAllShortcuts()
        {
            CreateShortcutInFolder(); 
            CreateShortcutOnDesktop(); 
        }

        private void CreateShortcutInFolder()
        {
            System.String shortcutPath = $"{RootFolder}\\SqlViewer.lnk"; 
            if ( !(System.IO.File.Exists(shortcutPath)) )
            {
                CreateShortcut(shortcutPath); 
            }
        }

        private void CreateShortcutOnDesktop()
        {
            System.String deskDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
            System.String shortcutPath = $"{deskDir}\\SqlViewer.lnk"; 
            if ( !(System.IO.File.Exists(shortcutPath)) )
            {
                System.Int32? fDesktopShortcut = null; // get this parameter from DB 
                if (fDesktopShortcut == null || fDesktopShortcut == 1)
                {
                    System.String msg = "Do you want to create a shortcut on Desktop?"; 
                    System.String caption = "Creating shortcut on Desktop"; 
                    var result = System.Windows.MessageBox.Show(msg, caption, 
                                MessageBoxButton.YesNoCancel, 
                                MessageBoxImage.Question); 
                    if (result == MessageBoxResult.Yes)
                    {
                        CreateShortcut(shortcutPath); 
                        fDesktopShortcut = 1; 
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        fDesktopShortcut = 0; 
                    }
                    else 
                    {
                        return; 
                    }
                    System.String sql = System.String.Empty; // for storing result in DB
                    MainVM.AppDbConnection.ExecuteSqlCommand(sql); 
                }
            }
        }

        private void CreateShortcut(System.String shortcutPath)
        {
            throw new System.NotSupportedException("Incorrect way to create a shortcut."); 

            using (var writer = new System.IO.StreamWriter(shortcutPath))
            {
                System.String app = System.Reflection.Assembly.GetExecutingAssembly().Location;
                //writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URI=file:///" + app);
                writer.WriteLine("IconIndex=0");
                System.String icon = $"{RootFolder}\\src\\Resources\\icon.ico";
                writer.WriteLine("IconFile=" + icon);
                writer.Flush();
            }
        }
        #endregion  // Shortcuts methods
    }
}
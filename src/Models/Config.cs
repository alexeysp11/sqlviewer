using System.Windows;
using SqlViewer.ViewModels; 
using SqlViewer.Models.DbConnections; 

namespace SqlViewer.Models
{
    public class Config
    {
        private MainVM MainVM { get; set; }

        private string RootFolder { get; set; }

        public Config(MainVM mainVM, string rootFolder)
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
            string dataFolderPath = $"{RootFolder}\\data"; 
            CreateFolderIfNotExists(dataFolderPath); 

            string sqlFolderPath = $"{RootFolder}\\src\\Queries\\Init"; 
            CreateLocalDbIfNotExists($"{dataFolderPath}\\app.db", $"{sqlFolderPath}\\InitAppDb.sql"); 
            CreateLocalDbIfNotExists($"{dataFolderPath}\\data.db", $"{sqlFolderPath}\\InitDataDb.sql"); 
            CreateLocalDbIfNotExists($"{dataFolderPath}\\test.db", $"{sqlFolderPath}\\InitTestDb.sql"); 
        }

        private void CreateFolderIfNotExists(string folderPath)
        {
            if ( !(System.IO.Directory.Exists(folderPath)) )
            {
                System.IO.Directory.CreateDirectory(folderPath); 
            }
        }

        private void CreateLocalDbIfNotExists(string localDbPath, string sqlScriptPath)
        {
            if ( !(System.IO.File.Exists(localDbPath)) )
            {
                string sql = this.MainVM.GetSqlRequest(sqlScriptPath); 
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
            string shortcutPath = $"{RootFolder}\\SqlViewer.lnk"; 
            if ( !(System.IO.File.Exists(shortcutPath)) )
            {
                CreateShortcut(shortcutPath); 
            }
        }

        private void CreateShortcutOnDesktop()
        {
            string deskDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
            string shortcutPath = $"{deskDir}\\SqlViewer.lnk"; 
            if ( !(System.IO.File.Exists(shortcutPath)) )
            {
                int? fDesktopShortcut = null; // get this parameter from DB 
                if (fDesktopShortcut == null || fDesktopShortcut == 1)
                {
                    string msg = "Do you want to create a shortcut on Desktop?"; 
                    string caption = "Creating shortcut on Desktop"; 
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
                    string sql = string.Empty; // for storing result in DB
                    MainVM.AppDbConnection.ExecuteSqlCommand(sql); 
                }
            }
        }

        private void CreateShortcut(string shortcutPath)
        {
            throw new System.NotSupportedException("Incorrect way to create a shortcut."); 

            using (var writer = new System.IO.StreamWriter(shortcutPath))
            {
                string app = System.Reflection.Assembly.GetExecutingAssembly().Location;
                //writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URI=file:///" + app);
                writer.WriteLine("IconIndex=0");
                string icon = $"{RootFolder}\\src\\Resources\\icon.ico";
                writer.WriteLine("IconFile=" + icon);
                writer.Flush();
            }
        }
        #endregion  // Shortcuts methods
    }
}
using System.Data; 
using System.Windows; 
using System.Windows.Controls; 
using SqlViewer.Helpers; 
using SqlViewer.Models; 
using SqlViewer.Models.DataStorage; 
using SqlViewer.Pages; 
using SqlViewer.Views; 
using UserControlsMenu = SqlViewer.UserControls.Menu; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.ViewModels
{
    /// <summary>
    /// ViewModel that deals with visual elements 
    /// </summary>
    public class VisualVM
    {
        #region Properties
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }

        /// <summary>
        /// Instance of Login view 
        /// </summary>
        public Window LoginView { get; set; }
        /// <summary>
        /// Instance of Settings view 
        /// </summary>
        public Window SettingsView { get; set; }
        /// <summary>
        /// Instance of Connections view 
        /// </summary>
        public Window ConnectionsView { get; set; }
        
        /// <summary>
        /// Instance of Menu 
        /// </summary>
        public UserControl Menu { get; set; }
        /// <summary>
        /// Instance of SQL page 
        /// </summary>
        public UserControl SqlPage { get; set; }
        /// <summary>
        /// Instance of Tables page 
        /// </summary>
        public UserControl TablesPage { get; set; }
        #endregion  // Properties

        /// <summary>
        /// Constructor of VisualVM class 
        /// </summary>
        public VisualVM(MainVM mainVM) => this.MainVM = mainVM; 

        #region Common UI methods 
        /// <summary>
        /// Initializes all UI elements 
        /// </summary>
        public void InitUI()
        {
            ((SettingsView)SettingsView).Init(); 

            ((UserControlsMenu)Menu).Init(); 

            ((SqlPage)SqlPage).Init(); 
            ((TablesPage)TablesPage).Init(); 
        }

        /// <summary>
        /// Initializes all UI elements 
        /// </summary>
        public void InitDbCredentials(DbCredentialsVE dbCredentialsVE)
        {
            bool isSqlite = dbCredentialsVE.cbActiveRdbms.Text == nameof(RdbmsEnum.SQLite); 

            dbCredentialsVE.tbServer.IsEnabled = isSqlite ? false : true; 
            dbCredentialsVE.tbPort.IsEnabled = isSqlite ? false : true; 
            dbCredentialsVE.tbSchema.IsEnabled = isSqlite ? false : true; 
            dbCredentialsVE.tbUsername.IsEnabled = isSqlite ? false : true; 
            dbCredentialsVE.pbPassword.IsEnabled = isSqlite ? false : true; 
            dbCredentialsVE.btnDatabase.IsEnabled = isSqlite ? true : false; 
            
            var color = isSqlite ? System.Windows.Media.Brushes.Gray : System.Windows.Media.Brushes.White; 
            dbCredentialsVE.tbServer.Background = color; 
            dbCredentialsVE.tbPort.Background = color; 
            dbCredentialsVE.tbSchema.Background = color; 
            dbCredentialsVE.tbUsername.Background = color; 
            dbCredentialsVE.pbPassword.Background = color; 

            dbCredentialsVE.tbServer.Text = string.Empty; 
            dbCredentialsVE.tbDatabase.Text = string.Empty; 
            dbCredentialsVE.tbPort.Text = string.Empty; 
            dbCredentialsVE.tbSchema.Text = string.Empty; 
            dbCredentialsVE.tbUsername.Text = string.Empty; 
            dbCredentialsVE.pbPassword.Password = string.Empty; 

            SqlViewer.Helpers.RepoHelper.AppSettingsRepo.DatabaseSettings.SetActiveRdbms(dbCredentialsVE.cbActiveRdbms.Text); 
        }

        public void DisplaySqlResult(DataTable resultCollection)
        {
            MainVM.MainWindow.SqlPage.dbgSqlResult.ItemsSource = resultCollection.DefaultView;
            MainVM.MainWindow.SqlPage.dbgSqlResult.Visibility = Visibility.Visible; 
            MainVM.MainWindow.SqlPage.dbgSqlResult.IsEnabled = true; 
        }

        public void DisplayActiveDb()
        {
            string dbNameStr = RepoHelper.AppSettingsRepo.DatabaseSettings.DbName; 

            MainVM.MainWindow.SqlPage.tblDbName.Text = dbNameStr;
            MainVM.MainWindow.TablesPage.tblDbName.Text = dbNameStr;

            if (MainVM.VisualVM.SettingsView != null)
                ((SettingsView)(MainVM.VisualVM.SettingsView)).tbDatabase.Text = dbNameStr;
            if (MainVM.VisualVM.LoginView != null)
                ((LoginView)(MainVM.VisualVM.LoginView)).tbDatabase.Text = dbNameStr; 
        }
        
        public void DisplayTablesInActiveDb()
        {
            MainVM.MainWindow.TablesPage.tvTables.Items.Clear();
            var dt = this.MainVM.CoreBranch.MainDbBranch.DatabasePreproc.DisplayTablesInDb(); 
            foreach (DataRow row in dt.Rows)
            {
                TreeViewItem item = new TreeViewItem(); 
                item.Header = row["name"].ToString();
                MainVM.MainWindow.TablesPage.tvTables.Items.Add(item); 
            }
            MainVM.MainWindow.TablesPage.tvTables.IsEnabled = true; 
            MainVM.MainWindow.TablesPage.tvTables.Visibility = Visibility.Visible; 
        }

        public void DisplayAllDataFromTable(string tableName)
        {
            MainVM.MainWindow.TablesPage.dgrAllData.ItemsSource = this.MainVM.CoreBranch.MainDbBranch.TablePreproc.GetAllDataFromTable(tableName).DefaultView;
        }
        public void DisplayColumnsOfTable(string tableName)
        {
            MainVM.MainWindow.TablesPage.dgrColumns.ItemsSource = this.MainVM.CoreBranch.MainDbBranch.TablePreproc.GetColumnsOfTable(tableName).DefaultView;
        }
        public void DisplayForeignKeys(string tableName)
        {
            MainVM.MainWindow.TablesPage.dgrForeignKeys.ItemsSource = this.MainVM.CoreBranch.MainDbBranch.TablePreproc.GetForeignKeys(tableName).DefaultView;
        }
        public void DisplayTriggers(string tableName)
        {
            MainVM.MainWindow.TablesPage.dgrTriggers.ItemsSource = this.MainVM.CoreBranch.MainDbBranch.TablePreproc.GetTriggers(tableName).DefaultView;
        }
        public void DisplayTableDefinition(string tableName)
        {
            MainVM.MainWindow.TablesPage.mtbSqlTableDefinition.Text = this.MainVM.CoreBranch.MainDbBranch.TablePreproc.GetTableDefinition(tableName);
        }
        #endregion  // Common UI methods 

        #region Views methods
        /// <summary>
        /// Opens view in the separate window 
        /// </summary>
        public void OpenView(string viewName)
        {
            try
            {
                var type = System.Type.GetType("SqlViewer.Views." + viewName); 
                var win = System.Activator.CreateInstance(type) as System.Windows.Window; 
                win.DataContext = this.MainVM;
                win.Show();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        /// <summary>
        /// Allows to open documentation in the default browser 
        /// </summary>
        public void OpenDocsInBrowser(string docName, string title, string filePath)
        {
            string msg = "Do you want to open " + docName + " in your browser?"; 
            if (System.Windows.MessageBox.Show(msg, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                try 
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = filePath;
                    process.Start();
                }
                catch (System.Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
                }
            }
        }
        #endregion  // Views methods

        #region Pages methods
        /// <summary>
        /// Redirects to SQL page 
        /// </summary>
        public void RedirectToSqlQuery()
        {
            HideAllPages();
            DisableAllPages();

            this.MainVM.MainWindow.SqlPage.Visibility = Visibility.Visible; 
            this.MainVM.MainWindow.SqlPage.IsEnabled = true; 
        }

        /// <summary>
        /// Redirects to Tables page 
        /// </summary>
        public void RedirectToTables()
        {
            DataTable dt = this.MainVM.CoreBranch.MainDbBranch.DatabasePreproc.DisplayTablesInDb();

            DisplayTablesInActiveDb(); 
            HideAllPages();
            DisableAllPages();

            this.MainVM.MainWindow.TablesPage.Visibility = Visibility.Visible; 
            this.MainVM.MainWindow.TablesPage.IsEnabled = true; 
        }

        /// <summary>
        /// Hides all pages and is used for redirecting to another page
        /// </summary>
        private void HideAllPages()
        {
            this.MainVM.MainWindow.SqlPage.Visibility = Visibility.Collapsed; 
            this.MainVM.MainWindow.TablesPage.Visibility = Visibility.Collapsed; 
        }

        /// <summary>
        /// Disables all pages and is used for redirecting to another page
        /// </summary>
        private void DisableAllPages()
        {
            this.MainVM.MainWindow.SqlPage.IsEnabled = false; 
            this.MainVM.MainWindow.TablesPage.IsEnabled = false; 
        }
        #endregion  // Pages methods
    }
}

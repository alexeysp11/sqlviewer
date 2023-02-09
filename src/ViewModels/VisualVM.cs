using System.Windows; 
using System.Windows.Controls; 
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

            SqlViewer.Helpers.RepoHelper.AppSettingsRepo.SetActiveRdbms(dbCredentialsVE.cbActiveRdbms.Text); 
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
            this.MainVM.DataVM.MainDbBranch.DisplayTablesInDb();

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

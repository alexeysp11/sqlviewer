using System.Windows; 
using System.Windows.Controls; 
using SqlViewer.Views; 
using SqlViewer.Pages; 
using UserControlsMenu = SqlViewer.UserControls.Menu; 

namespace SqlViewer.ViewModels
{
    public class VisualVM
    {
        private MainVM MainVM { get; set; }

        public Window LoginView { get; set; }
        public Window SettingsView { get; set; }
        public Window ConnectionsView { get; set; }
        
        public UserControl Menu { get; set; }
        public UserControl SqlPage { get; set; }
        public UserControl TablesPage { get; set; }

        public VisualVM(MainVM mainVM)
        {
            this.MainVM = mainVM; 
        }

        #region Common UI methods 
        public void InitUI()
        {
            ((SettingsView)SettingsView).Init(); 

            ((UserControlsMenu)Menu).Init(); 

            ((SqlPage)SqlPage).Init(); 
            ((TablesPage)TablesPage).Init(); 
        }
        #endregion  // Common UI methods 

        #region Views methods
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
        public void RedirectToSqlQuery()
        {
            HideAllPages();
            DisableAllPages();

            this.MainVM.MainWindow.SqlPage.Visibility = Visibility.Visible; 
            this.MainVM.MainWindow.SqlPage.IsEnabled = true; 
        }

        public void RedirectToTables()
        {
            this.MainVM.DataVM.MainDbBranch.DisplayTablesInDb();

            HideAllPages();
            DisableAllPages();

            this.MainVM.MainWindow.TablesPage.Visibility = Visibility.Visible; 
            this.MainVM.MainWindow.TablesPage.IsEnabled = true; 
        }

        private void HideAllPages()
        {
            this.MainVM.MainWindow.SqlPage.Visibility = Visibility.Collapsed; 
            this.MainVM.MainWindow.TablesPage.Visibility = Visibility.Collapsed; 
        }

        private void DisableAllPages()
        {
            this.MainVM.MainWindow.TablesPage.IsEnabled = false; 
            this.MainVM.MainWindow.TablesPage.IsEnabled = false; 
        }
        #endregion  // Pages methods
    }
}

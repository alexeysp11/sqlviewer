using System.Data; 
using System.Windows; 
using System.Windows.Controls; 
using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 
using SqlViewerDatabase.DbConnections; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.DbPreproc
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabasePreproc
    {
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }

        /// <summary>
        /// Constructor of DatabasePreproc
        /// </summary>
        public DatabasePreproc(MainVM mainVM)
        {
            this.MainVM = mainVM; 
        }

        /// <summary>
        /// Creates database using UserRdbmsPreproc
        /// </summary>
        public void CreateDb()
        {
            try
            {
                MainVM.DataVM.MainDbBranch.CenterDbPreprocFactory.DatabasePreprocFactory.GetDbDatabasePreproc().CreateDb();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Opens database using UserRdbmsPreproc
        /// </summary>
        public void OpenDb()
        {
            try
            {
                MainVM.DataVM.MainDbBranch.CenterDbPreprocFactory.DatabasePreprocFactory.GetDbDatabasePreproc().OpenDb();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Displays all tables that a database contains 
        /// </summary>
        public void DisplayTablesInDb()
        {
            var mainDbBranch = MainVM.DataVM.MainDbBranch; 
            if (mainDbBranch.DbConnectionPreproc.UserDbConnection == null)
                return; 
            try
            {
                DataTable dt = mainDbBranch.CenterDbPreprocFactory.DatabasePreprocFactory.GetDbDatabasePreproc().DisplayTablesInDb();
                MainVM.MainWindow.TablesPage.tvTables.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    TreeViewItem item = new TreeViewItem(); 
                    item.Header = row["name"].ToString();
                    MainVM.MainWindow.TablesPage.tvTables.Items.Add(item); 
                }
                MainVM.MainWindow.TablesPage.tvTables.IsEnabled = true; 
                MainVM.MainWindow.TablesPage.tvTables.Visibility = Visibility.Visible; 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
    }
}

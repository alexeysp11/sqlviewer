using System.Data;
using System.Windows;
using SqlViewer.Helpers;
using SqlViewer.ViewModels;

namespace SqlViewer.Models.DbPreproc
{
    /// <summary>
    /// Performs request related operations 
    /// </summary>
    public class RequestPreproc
    {
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }

        /// <summary>
        /// Constructor of RequestPreproc
        /// </summary>
        public RequestPreproc(MainVM mainVM) => this.MainVM = mainVM; 

        /// <summary>
        /// Sends SQL quty to database 
        /// </summary>
        public void SendSqlRequest()
        {
            try
            {
                if (MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection == null)
                    throw new System.Exception("Database is not opened."); 

                DataTable resultCollection = MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(MainVM.MainWindow.SqlPage.mtbSqlRequest.Text);
                MainVM.MainWindow.SqlPage.dbgSqlResult.ItemsSource = resultCollection.DefaultView;

                MainVM.MainWindow.SqlPage.dbgSqlResult.Visibility = Visibility.Visible; 
                MainVM.MainWindow.SqlPage.dbgSqlResult.IsEnabled = true; 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        /// <summary>
        /// Sends SQL quty to database and gets a result in a form of DataTable 
        /// </summary>
        public DataTable SendSqlRequest(string sql)
        {
            DataTable dt = new DataTable(); 
            try
            {
                if (MainVM.DataVM.MainDbBranch.DbConnectionPreproc.AppDbConnection == null)
                    throw new System.Exception("System RDBMS is not assigned."); 
                return MainVM.DataVM.MainDbBranch.DbConnectionPreproc.AppDbConnection.ExecuteSqlCommand(sql);
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
            return dt; 
        }

        /// <summary>
        /// Gets SQL query from a local file 
        /// </summary>
        public string GetSqlRequestFromFile(string filename)
        {
            string sqlRequest = string.Empty; 
            try
            {
                sqlRequest = System.IO.File.ReadAllText(System.IO.Path.Combine(SettingsHelper.GetRootFolder(), $"queries/{filename}")); 
            }
            catch (System.Exception ex) 
            {
                throw ex; 
            }
            return sqlRequest; 
        }
    }
}

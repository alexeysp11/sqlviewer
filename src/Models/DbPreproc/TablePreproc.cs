using System.Data; 
using System.Windows; 
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
    public class TablePreproc 
    {
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }

        public TablePreproc(MainVM mainVM) => this.MainVM = mainVM; 

        /// <summary>
        /// Gets all data from a table 
        /// </summary>
        public void GetAllDataFromTable(string tableName)
        {
            try
            {
                string sqlRequest = $"SELECT * FROM {tableName}"; 
                MainVM.MainWindow.TablesPage.dgrAllData.ItemsSource = MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        /// <summary>
        /// Gets info about all the columns that a table contains 
        /// </summary>
        public void GetColumnsOfTable(string tableName)
        {
            try
            {
                string sqlRequest = MainVM.DataVM.MainDbBranch.CenterDbPreprocFactory.TablePreprocFactory.GetDbTablePreproc().GetColumnsOfTableSql(tableName); 
                if (string.IsNullOrEmpty(sqlRequest))
                    throw new System.Exception("SQL request could not be empty after reading a file"); 
                MainVM.MainWindow.TablesPage.dgrColumns.ItemsSource = MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        /// <summary>
        /// Gets info about all the foreign keys that a table contains 
        /// </summary>
        public void GetForeignKeys(string tableName)
        {
            try
            {
                string sqlRequest = MainVM.DataVM.MainDbBranch.CenterDbPreprocFactory.TablePreprocFactory.GetDbTablePreproc().GetForeignKeysSql(tableName); 
                if (string.IsNullOrEmpty(sqlRequest))
                    throw new System.Exception("SQL request could not be empty after reading a file"); 
                MainVM.MainWindow.TablesPage.dgrForeignKeys.ItemsSource = MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        /// <summary>
        /// Gets info about all the triggers that a table contains 
        /// </summary>
        public void GetTriggers(string tableName)
        {
            try
            {
                string sqlRequest = MainVM.DataVM.MainDbBranch.CenterDbPreprocFactory.TablePreprocFactory.GetDbTablePreproc().GetTriggersSql(tableName); 
                if (string.IsNullOrEmpty(sqlRequest))
                    throw new System.Exception("SQL request could not be empty after reading a file"); 
                MainVM.MainWindow.TablesPage.dgrTriggers.ItemsSource = MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        /// <summary>
        /// Gets SQL query for recreating a table  
        /// </summary>
        public void GetSqlDefinition(string tableName)
        {
            try
            {
                string sqlRequest = MainVM.DataVM.MainDbBranch.CenterDbPreprocFactory.TablePreprocFactory.GetDbTablePreproc().GetSqlDefinitionSql(tableName); 
                if (string.IsNullOrEmpty(sqlRequest))
                    throw new System.Exception("SQL request could not be empty after reading a file"); 
                DataTable dt = MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest);
                if (dt.Rows.Count > 0) 
                {
                    DataRow row = dt.Rows[0];
                    MainVM.MainWindow.TablesPage.mtbSqlTableDefinition.Text = row["sql"].ToString();
                }
                else 
                {
                    MainVM.MainWindow.TablesPage.mtbSqlTableDefinition.Text = string.Empty;
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        
    }
}

using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using SqlViewer.Models.DbConnections;
using SqlViewer.ViewModels;
using SqlViewer.Views;
using SqlViewer.Helpers; 
using SqlViewer.Entities.UserControlsEntities; 
using LanguageEnum = SqlViewer.Enums.Common.Language; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 
using ICommonDbConnectionSV = SqlViewer.Models.DbConnections.ICommonDbConnection; 

namespace SqlViewer.UserControls
{
    /// <summary>
    /// Interaction logic for Connection.xaml
    /// </summary>
    public partial class Connection : UserControl
    {
        private MainVM MainVM { get; set; }
        private ConnectionEntity ConnectionEntity { get; set; }

        private int OrdinalNum = 0; 

        public Connection()
        {
            InitializeComponent();

            Loaded += (o, e) => 
            {
                this.MainVM = (MainVM)this.DataContext; 
                this.ConnectionEntity = this.MainVM.Translator.ConnectionEntity; 
                Init(); 
            }; 
        }
        
        #region Initialization
        private void Init()
        {
            lblActiveRdbms.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(ConnectionEntity.ActiveRdbmsField.Translation) ? ConnectionEntity.ActiveRdbmsField.English + ":" : ConnectionEntity.ActiveRdbmsField.Translation + ":"; 
            btnConnectionExecute.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(ConnectionEntity.ExecuteField.Translation) ? ConnectionEntity.ExecuteField.English : ConnectionEntity.ExecuteField.Translation; 
            btnConnectionTransfer.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(ConnectionEntity.TransferField.Translation) ? ConnectionEntity.TransferField.English : ConnectionEntity.TransferField.Translation; 
        }

        public void SetOrdinalNum(int ordinalNum) 
        {
            try
            {
                if (ordinalNum != 1 && ordinalNum != 2)
                    throw new System.Exception("Incorrect ordinalNum was passed during initialization of UserControls.Connection: '" + ordinalNum + "'"); 
                OrdinalNum = ordinalNum; 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        #endregion  // Initialization

        #region Event processing
        private void btnOpenSqliteDataSource_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog(); 
                ofd.Filter = SettingsHelper.GetFilterFileSystemDb();
                if (ofd.ShowDialog() == true) {}

                string path = ofd.FileName; 
                if (path == string.Empty) return; 
                tbDataSource.Text = path;
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        private void btnDataSource_Clicked(object sender, RoutedEventArgs e)
        {
            string msg = @"SQLite: specify the full path to the local database (for example, 'C:\projects\sqlviewer\data\app.db').

PostgreSQL: specify server, username, database, port and password (for example, 'Server=localhost;Username=username;Database=database;Port=800;Password=password').

MySQL: specify server, database, username, password (for example, Server=localhost; database=your_db; UID=username; password=password). 

Oracle: specify protocol, host, port, service name, user ID and password (for example, 'Data Source=(DESCRIPTION =
    (ADDRESS_LIST =
      (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))
    )
    (CONNECT_DATA =
      (SERVICE_NAME = service_name)
    )
  ); User ID=user_id;Password=password;')."; 
            System.Windows.MessageBox.Show(msg, "Instruction: How to form DataSource (DS) string", MessageBoxButton.OK, MessageBoxImage.Information); 
        }

        private void cbActiveRdbms_DropDownClosed(object sender, System.EventArgs e)
        {
            tbDataSource.Text = System.String.Empty; 
            btnOpenSqliteDataSource.IsEnabled = cbActiveRdbms.Text == "SQLite" ? true : false; 
            tbActiveRdbms.Text = cbActiveRdbms.Text; 
        }

        private void btnConnectionExecute_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                ICommonDbConnectionSV dbConnection = GetDbConnection(); 
                switch (OrdinalNum)
                {
                    case 1:
                        this.MainVM.DataVM.DbInterconnection.SetDbConnection1(dbConnection); 
                        break;
                
                    case 2:
                        this.MainVM.DataVM.DbInterconnection.SetDbConnection2(dbConnection); 
                        break;

                    default: 
                        throw new System.Exception("Incorrect OrdinalNum in UserControls.Connection: '" + OrdinalNum + "'"); 
                        break; 
                }
                dbgSqlResult.ItemsSource = OrdinalNum == 1 ? this.MainVM.DataVM.DbInterconnection.DbConnection1.ExecuteSqlCommand(mtbSqlRequest.Text).DefaultView : this.MainVM.DataVM.DbInterconnection.DbConnection2.ExecuteSqlCommand(mtbSqlRequest.Text).DefaultView; 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        private void btnConnectionTransfer_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                if (OrdinalNum != 1 && OrdinalNum != 2)
                    throw new System.Exception("Incorrect OrdinalNum in UserControls.Connection: '" + OrdinalNum + "'"); 

                string tableName = SettingsHelper.GetTmpTableTransfer(); 
                TransferData(tableName); 
                System.Windows.MessageBox.Show("Data successfully transferred!\nTable name: " + tableName, "Information", MessageBoxButton.OK, MessageBoxImage.Information); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        #endregion  // Event processing

        #region Database methods
        private ICommonDbConnectionSV GetDbConnection()
        {
            ICommonDbConnectionSV dbConnection; 
            switch (cbActiveRdbms.Text)
            {
                case "SQLite":
                    dbConnection = new SqliteDbConnection(tbDataSource.Text); 
                    break;
            
                case "PostgreSQL":
                    dbConnection = new PgDbConnection(tbDataSource.Text); 
                    break;
            
                case "MySQL":
                    dbConnection = new MysqlDbConnection(tbDataSource.Text); 
                    break;
            
                case "Oracle":
                    dbConnection = new OracleDbConnection(tbDataSource.Text); 
                    break;

                default: 
                    throw new System.Exception("Incorrect ActiveRdbms in UserControls.Connection: '" + cbActiveRdbms.Text + "'"); 
                    break; 
            }
            return dbConnection; 
        }

        private void TransferData(string tableName)
        {
            try
            {
                ConnectionsView connectionView = (ConnectionsView)(this.MainVM.VisualVM.ConnectionsView); 
                if ( !(connectionView.CheckDataSources()) ) 
                    throw new System.Exception("All DataSources should be assigned"); 
                if ( !(connectionView.CheckDataGrids()) ) 
                    throw new System.Exception("None of DataGrids should be empty"); 
                
                ICommonDbConnectionSV dbConnection = OrdinalNum == 1 ? this.MainVM.DataVM.DbInterconnection.DbConnection2 : this.MainVM.DataVM.DbInterconnection.DbConnection1; 
                DataTable dt = ((DataView)(dbgSqlResult.ItemsSource)).Table; 
                dbConnection.ExecuteSqlCommand(dbConnection.GetSqlFromDataTable(dt, tableName)); 
            }
            catch(System.Exception ex)
            {
                throw ex; 
            }
        }
        #endregion  // Database methods
    }
}

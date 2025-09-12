using System.Data;
using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;
using SqlViewer.Views;
using SqlViewer.Helpers;
using SqlViewer.Entities.UserControlsEntities;
using LanguageEnum = SqlViewer.Enums.Common.Language;
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms;
using ICommonDbConnectionSV = WorkflowLib.Shared.DbConnections.ICommonDbConnection;

namespace SqlViewer.UserControls
{
    /// <summary>
    /// Interaction logic for Connection.xaml
    /// </summary>
    public partial class Connection : UserControl
    {
        /// <summary>
        /// Main ViewModel 
        /// </summary>
        private MainVM MainVM { get; set; }
        /// <summary>
        /// Entity for storing data for translating visual elements on the UserControl 
        /// </summary>
        private ConnectionEntity ConnectionEntity { get; set; }

        /// <summary>
        /// Ordinal number that allows to distinguish what database source it is necessary to connect to 
        /// </summary>
        private int OrdinalNum = 0; 

        /// <summary>
        /// Constructor of Connection
        /// </summary>
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
        /// <summary>
        /// Initializes the UserControl 
        /// </summary>
        private void Init()
        {
            lblActiveRdbms.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(ConnectionEntity.ActiveRdbmsField.Translation) ? ConnectionEntity.ActiveRdbmsField.English + ":" : ConnectionEntity.ActiveRdbmsField.Translation + ":"; 
            btnConnectionExecute.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(ConnectionEntity.ExecuteField.Translation) ? ConnectionEntity.ExecuteField.English : ConnectionEntity.ExecuteField.Translation; 
            btnConnectionTransfer.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(ConnectionEntity.TransferField.Translation) ? ConnectionEntity.TransferField.English : ConnectionEntity.TransferField.Translation; 
        }

        /// <summary>
        /// Sets ordinal number of the UserControl 
        /// </summary>
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
        /// <summary>
        /// Opens local database if SQLite was selected 
        /// </summary>
        private void btnOpenSqliteDataSource_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = SqlViewer.Helpers.FileSysHelper.OpenLocalFile(); 
                if (path == string.Empty) return; 
                tbDataSource.Text = path;
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        /// <summary>
        /// Displays an instruction on how to form DataSource string
        /// </summary>
        private void btnDataSource_Clicked(object sender, RoutedEventArgs e)
        {
            string msg = SqlViewer.Helpers.SettingsHelper.GetHelpDataSourceString(); 
            System.Windows.MessageBox.Show(msg, "Instruction: How to form DataSource (DS) string", MessageBoxButton.OK, MessageBoxImage.Information); 
        }

        /// <summary>
        /// Reinitializes visual elements when active RDBMS was selected 
        /// </summary>
        private void cbActiveRdbms_DropDownClosed(object sender, System.EventArgs e)
        {
            tbDataSource.Text = System.String.Empty; 
            btnOpenSqliteDataSource.IsEnabled = cbActiveRdbms.Text == nameof(RdbmsEnum.SQLite) ? true : false; 
            tbActiveRdbms.Text = cbActiveRdbms.Text; 
        }

        /// <summary>
        /// Executes SQL script written in Multiline textbox 
        /// </summary>
        private void btnConnectionExecute_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                var di = this.MainVM.DataVM.InterDbBranch.DbInterconnection; 
                ICommonDbConnectionSV dbConnection = di.GetDbConnection(cbActiveRdbms.Text, tbDataSource.Text); 
                switch (OrdinalNum)
                {
                    case 1:
                        di.SetInterDbConnection1(dbConnection); 
                        break;
                
                    case 2:
                        di.SetInterDbConnection2(dbConnection); 
                        break;

                    default: 
                        throw new System.Exception("Incorrect OrdinalNum in UserControls.Connection: '" + OrdinalNum + "'"); 
                }
                dbgSqlResult.ItemsSource = OrdinalNum == 1 ? di.InterDbConnection1.ExecuteSqlCommand(mtbSqlRequest.Text).DefaultView : di.InterDbConnection2.ExecuteSqlCommand(mtbSqlRequest.Text).DefaultView; 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        /// <summary>
        /// Transfers data from one database to another when the Transfer button was clicked 
        /// </summary>
        private void btnConnectionTransfer_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                if (OrdinalNum != 1 && OrdinalNum != 2)
                    throw new System.Exception("Incorrect OrdinalNum in UserControls.Connection: '" + OrdinalNum + "'"); 

                string tableName = SettingsHelper.GetTmpTableTransferString(); 
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
        /// <summary>
        /// Transfers data from one database to another 
        /// </summary>
        private void TransferData(string tableName)
        {
            try
            {
                ConnectionsView connectionView = (ConnectionsView)(this.MainVM.VisualVM.ConnectionsView); 
                if ( !(connectionView.CheckDataSources()) ) 
                    throw new System.Exception("All DataSources should be assigned"); 
                if ( !(connectionView.CheckDataGrids()) ) 
                    throw new System.Exception("None of DataGrids should be empty"); 
                
                ICommonDbConnectionSV dbConnection = OrdinalNum == 1 ? this.MainVM.DataVM.InterDbBranch.DbInterconnection.InterDbConnection2 : this.MainVM.DataVM.InterDbBranch.DbInterconnection.InterDbConnection1; 
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

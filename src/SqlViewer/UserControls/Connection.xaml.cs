using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using SqlViewer.ViewModels;
using SqlViewer.Views;
using SqlViewer.Helpers;
using SqlViewer.Entities.UserControlsEntities;
using LanguageEnum = SqlViewer.Enums.Common.Language;

namespace SqlViewer.UserControls;

/// <summary>
/// Interaction logic for Connection.xaml
/// </summary>
public partial class Connection : UserControl
{
    private MainVM MainVM { get; set; }
    private ConnectionEntity ConnectionEntity { get; set; }

    private int OrdinalNum;

    public Connection()
    {
        InitializeComponent();

        Loaded += (o, e) =>
        {
            MainVM = (MainVM)DataContext;
            ConnectionEntity = MainVM.Translator.ConnectionEntity;
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
                throw new Exception("Incorrect ordinalNum was passed during initialization of UserControls.Connection: '" + ordinalNum + "'");
            OrdinalNum = ordinalNum;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion  // Initialization

    #region Event processing
    private void btnOpenSqliteDataSource_Clicked(object sender, RoutedEventArgs e)
    {
        try
        {
            OpenFileDialog ofd = new()
            {
                Filter = SettingsHelper.FilterFileSystemDb
            };
            if (ofd.ShowDialog() == true) { }

            string path = ofd.FileName;
            if (path == string.Empty) return;
            tbDataSource.Text = path;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void btnDataSource_Clicked(object sender, RoutedEventArgs e)
    {
        string msg = @"SQLite: specify the full path to the local database (for example, 'C:\projects\sqlviewer\data\app.db').

PostgreSQL: specify server, username, database, port and password (for example, 'Server=localhost;Username=username;Database=database;Port=800;Password=password').

Oracle: specify protocol, host, port, service name, user ID and password (for example, 'Data Source=(DESCRIPTION =
    (ADDRESS_LIST =
      (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))
    )
    (CONNECT_DATA =
      (SERVICE_NAME = service_name)
    )
  ); User ID=user_id;Password=password;')";
        MessageBox.Show(msg, "Instruction: How to form DataSource (DS) string", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void cbActiveRdbms_DropDownClosed(object sender, EventArgs e)
    {
        tbDataSource.Text = string.Empty;
        btnOpenSqliteDataSource.IsEnabled = cbActiveRdbms.Text == "SQLite" ? true : false;
        tbActiveRdbms.Text = cbActiveRdbms.Text;
    }

    private void btnConnectionExecute_Clicked(object sender, EventArgs e)
    {
        try
        {
            //ICommonDbConnectionSV dbConnection = GetDbConnection();
            switch (OrdinalNum)
            {
                case 1:
                    //MainVM.DataVM.DbInterconnection.SetDbConnection1(dbConnection);
                    break;

                case 2:
                    //MainVM.DataVM.DbInterconnection.SetDbConnection2(dbConnection);
                    break;

                default:
                    throw new Exception("Incorrect OrdinalNum in UserControls.Connection: '" + OrdinalNum + "'");
            }
            //dbgSqlResult.ItemsSource = OrdinalNum == 1 ? MainVM.DataVM.DbInterconnection.DbConnection1.ExecuteSqlCommand(mtbSqlRequest.Text).DefaultView : MainVM.DataVM.DbInterconnection.DbConnection2.ExecuteSqlCommand(mtbSqlRequest.Text).DefaultView;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void btnConnectionTransfer_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (OrdinalNum != 1 && OrdinalNum != 2)
                throw new Exception("Incorrect OrdinalNum in UserControls.Connection: '" + OrdinalNum + "'");

            string tableName = SettingsHelper.GetTmpTableTransfer();
            TransferData(tableName);
            MessageBox.Show("Data successfully transferred!\nTable name: " + tableName, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion  // Event processing

    #region Database methods

#pragma warning disable IDE0060 // Remove unused parameter
    private void TransferData(string tableName)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        ConnectionsView connectionView = (ConnectionsView)MainVM.VisualVM.ConnectionsView;
        if (!connectionView.CheckDataSources())
            throw new Exception("All DataSources should be assigned");
        if (!ConnectionsView.CheckDataGrids())
            throw new Exception("None of DataGrids should be empty");

        //ICommonDbConnectionSV dbConnection = OrdinalNum == 1
        //    ? MainVM.DataVM.DbInterconnection.DbConnection2
        //    : MainVM.DataVM.DbInterconnection.DbConnection1;
        //DataTable dt = ((DataView)dbgSqlResult.ItemsSource).Table;
        //dbConnection.ExecuteSqlCommand(dbConnection.GetSqlFromDataTable(dt, tableName));
    }
    #endregion  // Database methods
}

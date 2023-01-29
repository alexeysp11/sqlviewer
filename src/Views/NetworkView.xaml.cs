using System.Windows;
using System.Windows.Controls;
using SqlViewer.Helpers; 
using SqlViewer.ViewModels;
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Views
{
    /// <summary>
    /// Interaction logic for NetworkView.xaml
    /// </summary>
    public partial class NetworkView : Window
    {
        private MainVM MainVM { get; set; }

        public NetworkView()
        {
            InitializeComponent();

            Loaded += (o, e) => 
            {
                this.MainVM = (MainVM)this.DataContext; 
                //this.ConnectionEntity = this.MainVM.Translator.ConnectionEntity; 
                Init(); 
                GetLocalIp(); 

                InitCommunicationProtocol(); 
                //InitTransferProtocol(); 
            }; 
        }

        private void Init()
        {
            rbDataTypeActiveRdbms.IsChecked = true; 
            rbLoggingOff.IsChecked = true; 
            rbAutoSaveInDbOff.IsChecked = true; 

            lblMessage.Content = "OK"; 
        }
        private void GetLocalIp()
        {
            tbThisPcAddress.Text = this.MainVM.DataVM.NetworkBranch.GetLocalIp(); 
        }
        private void InitCommunicationProtocol()
        {
            try
            {
                this.MainVM.DataVM.NetworkBranch.InitCommunicationProtocol(cbCommunicationProtocol.Text); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message); 
            }
        }
        private void InitTransferProtocol()
        {
            this.MainVM.DataVM.NetworkBranch.InitTransferProtocol(cbTransferProtocol.Text); 
        }

        private void btnPing_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                bool pingable = this.MainVM.DataVM.NetworkBranch.PingHost(tbHost.Text); 
                lblMessage.Content = "Ping '" + tbHost.Text + "': " + (pingable ? "success" : "failed"); 
                System.Windows.MessageBox.Show(lblMessage.Content.ToString()); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        private void btnConnect_Clicked(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("btnConnect_Clicked"); 
        }
        private void btnOpenDataSource_Clicked(object sender, System.EventArgs e)
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
        private void btnOpenFile_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                string path = SqlViewer.Helpers.FileSysHelper.OpenLocalFile(); 
                if (path == string.Empty) return; 
                tbFile.Text = path;
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        private void btnUploadFile_Clicked(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("btnUploadFile_Clicked"); 
        }
        private void btnDownloadFile_Clicked(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("btnDownloadFile_Clicked"); 
        }
        private void btnExecute_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                var di = this.MainVM.DataVM.NetworkBranch.DbInterconnection; 
                ICommonDbConnectionSV dbConnection = di.GetDbConnection(cbActiveRdbms.Text, tbDataSource.Text); 
                di.SetNetworkDbConnection(dbConnection); 
                dbgSqlResult.ItemsSource = di.NetworkDbConnection.ExecuteSqlCommand(mtbSqlRequest.Text).DefaultView; 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        private void btnTransfer_Clicked(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("btnTransfer_Clicked"); 
        }
        private void btnSave_Clicked(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("btnSave_Clicked"); 
        }
        private void btnClear_Clicked(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("btnClear_Clicked"); 
        }
        private void btnCancel_Clicked(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("btnCancel_Clicked"); 
        }

        private void btnWatchLogs_Clicked(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("btnWatchLogs_Clicked"); 
        }
        private void btnSaveLogs_Clicked(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("btnSaveLogs_Clicked"); 
        }

        private void btnHelpDataSource_Clicked(object sender, RoutedEventArgs e)
        {
            string msg = SqlViewer.Helpers.SettingsHelper.GetHelpDataSourceString(); 
            System.Windows.MessageBox.Show(msg, "Instruction: How to form DataSource (DS) string", MessageBoxButton.OK, MessageBoxImage.Information); 
        }
        private void btnHelpThisPcAddress_Clicked(object sender, RoutedEventArgs e)
        {
            string msg = SqlViewer.Helpers.SettingsHelper.GetHelpThisPcAddressString(); 
            System.Windows.MessageBox.Show(msg, "Instruction: Network address of this PC", MessageBoxButton.OK, MessageBoxImage.Information); 
        }
        private void btnCommunicationProtocolHelp_Clicked(object sender, System.EventArgs e)
        {
            string msg = SqlViewer.Helpers.SettingsHelper.GetHelpCommunicationProtocolString(); 
            System.Windows.MessageBox.Show(msg, "Instruction: Communication protocol", MessageBoxButton.OK, MessageBoxImage.Information); 
        }
        private void btnTransferProtocolHelp_Clicked(object sender, System.EventArgs e)
        {
            string msg = SqlViewer.Helpers.SettingsHelper.GetHelpTransferProtocolString(); 
            System.Windows.MessageBox.Show(msg, "Instruction: Transfer protocol", MessageBoxButton.OK, MessageBoxImage.Information); 
        }

        private void cbActiveRdbms_DropDownClosed(object sender, System.EventArgs e)
        {
            tbDataSource.Text = System.String.Empty; 
            btnOpenDataSource.IsEnabled = cbActiveRdbms.Text == "SQLite" ? true : false; 
            tbActiveRdbms.Text = cbActiveRdbms.Text; 
        }
        private void cbCustomDocType_DropDownClosed(object sender, System.EventArgs e)
        {
            tbFile.Text = System.String.Empty; 
        }
        private void cbCommunicationProtocol_DropDownClosed(object sender, System.EventArgs e)
        {
            InitCommunicationProtocol(); 
        }
        private void cbTransferProtocol_DropDownClosed(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("cbTransferProtocol_DropDownClosed"); 
        }

        private void rbDataTypeChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            RadioButton li = (sender as RadioButton);
            switch (li.Name.ToString())
            {
                case "rbDataTypeActiveRdbms":
                    spActiveRdbms.IsEnabled = true; 
                    spCustomDocType.IsEnabled = false; 
                    spDataSource.IsEnabled = true; 
                    spFile.IsEnabled = false; 
                    spFileBtns.IsEnabled = false; 
                    btnExecute.IsEnabled = true; 
                    btnTransfer.IsEnabled = true; 
                    btnSave.IsEnabled = true; 
                    spAutoSaveInDb.IsEnabled = true; 

                    tbFile.Text = string.Empty; 
                    break;
                    
                case "rbDataTypeCustomDocType":
                    spActiveRdbms.IsEnabled = false; 
                    spCustomDocType.IsEnabled = true; 
                    spDataSource.IsEnabled = false; 
                    spFile.IsEnabled = true; 
                    spFileBtns.IsEnabled = true; 
                    btnExecute.IsEnabled = false; 
                    btnTransfer.IsEnabled = false; 
                    btnSave.IsEnabled = false; 
                    spAutoSaveInDb.IsEnabled = false; 

                    tbDataSource.Text = string.Empty; 
                    tbAutoSaveInDb.Text = string.Empty; 
                    break;
                    
                default: 
                    System.Windows.MessageBox.Show($"Incorrect name of RadioButton: '{li.Name.ToString()}'", "Exception"); 
                    break; 
            }
        }
        private void rbLoggingChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            RadioButton li = (sender as RadioButton);
            System.Windows.MessageBox.Show("rbLoggingChecked " + li.Content.ToString()); 
        }
        private void rbAutoSaveInDbChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            RadioButton li = (sender as RadioButton);
            System.Windows.MessageBox.Show("rbAutoSaveInDbChecked " + li.Content.ToString()); 
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

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
            }; 
        }

        private void Init()
        {
            rbDataTypeActiveRdbms.IsChecked = true; 
            rbLoggingOff.IsChecked = true; 

            lblMessage.Content = "OK"; 
        }

        private void btnPing_Clicked(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("btnPing_Clicked"); 
        }
        private void btnConnect_Clicked(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("btnConnect_Clicked"); 
        }
        private void btnUpload_Clicked(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("btnUpload_Clicked"); 
        }
        private void btnDownload_Clicked(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("btnDownload_Clicked"); 
        }
        private void btnExecute_Clicked(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("btnExecute_Clicked"); 
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

        private void cbActiveRdbms_DropDownClosed(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("cbActiveRdbms_DropDownClosed"); 
        }
        private void cbCustomDocType_DropDownClosed(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("cbCustomDocType_DropDownClosed"); 
        }
        private void cbNetworkProtocol_DropDownClosed(object sender, System.EventArgs e)
        {
            System.Windows.MessageBox.Show("cbNetworkProtocol_DropDownClosed"); 
        }

        private void rbDataTypeChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            RadioButton li = (sender as RadioButton);
            switch (li.Name.ToString())
            {
                case "rbDataTypeActiveRdbms":
                    spActiveRdbms.IsEnabled = true; 
                    spCustomDocType.IsEnabled = false; 
                    btnUpload.IsEnabled = false; 
                    btnDownload.IsEnabled = false; 
                    break;
                    
                case "rbDataTypeCustomDocType":
                    spActiveRdbms.IsEnabled = false; 
                    spCustomDocType.IsEnabled = true; 
                    btnUpload.IsEnabled = true; 
                    btnDownload.IsEnabled = true; 
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
    }
}

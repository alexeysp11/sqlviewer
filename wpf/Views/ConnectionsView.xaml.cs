using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.Views
{
    /// <summary>
    /// Interaction logic for ConnectionsView.xaml
    /// </summary>
    public partial class ConnectionsView : Window
    {
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }

        /// <summary>
        /// Constructor of ConnectionsView
        /// </summary>
        public ConnectionsView()
        {
            InitializeComponent();

            Loaded += (o, e) => {
                this.MainVM = (MainVM)this.DataContext; 
                this.MainVM.VisualVM.ConnectionsView = this; 

                Connection1.SetOrdinalNum(1); 
                Connection2.SetOrdinalNum(2); 
            }; 
        }

        /// <summary>
        /// Checks if data sources are specified by user 
        /// </summary>
        public bool CheckDataSources()
        {
            return !(string.IsNullOrEmpty(Connection1.tbDataSource.Text)) && !(string.IsNullOrEmpty(Connection2.tbDataSource.Text)); 
        }

        /// <summary>
        /// Checks if each of the DataGrid contains data to transfer 
        /// </summary>
        public bool CheckDataGrids()
        {
            return true; //!(string.IsNullOrEmpty(Connection1.tbDataSource.Text)) && !(string.IsNullOrEmpty(Connection2.tbDataSource.Text)); 
        }

        /// <summary>
        /// Event handler which sets the reference to itself in MainVM to null, when the view is closing 
        /// </summary>
        private void ConnectionsView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainVM.VisualVM.ConnectionsView = null; 
        }
    }
}

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
        /// 
        /// </summary>
        private MainVM MainVM { get; set; }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public bool CheckDataSources()
        {
            return !(string.IsNullOrEmpty(Connection1.tbDataSource.Text)) && !(string.IsNullOrEmpty(Connection2.tbDataSource.Text)); 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CheckDataGrids()
        {
            return true; //!(string.IsNullOrEmpty(Connection1.tbDataSource.Text)) && !(string.IsNullOrEmpty(Connection2.tbDataSource.Text)); 
        }

       /// <summary>
        /// 
        /// </summary>
        private void ConnectionsView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainVM)this.DataContext).VisualVM.ConnectionsView = null; 
        }
    }
}

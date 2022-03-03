using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private MainVM MainVM { get; set; }

        private bool IsLoggedIn { get; set; } = false; 

        public LoginView()
        {
            InitializeComponent();

            Loaded += (o, e) => 
            {
                this.MainVM = ((MainVM)this.DataContext); 
            }; 
        }

        private void btnLogIn_Clicked(object sender, RoutedEventArgs e)
        {
            this.IsLoggedIn = true; 

            string sql = this.MainVM.GetSqlRequest("App/UpdateSettingsDb.sql"); 
            sql = string.Format(sql, "SQLite", cbActiveRdbms.Text, tbDatabase.Text, 
                tbSchema.Text, tbUsername.Text, pbPassword.Password); 
            this.MainVM.SendSqlRequest(sql); 
            this.MainVM.InitAppRepository(); 
            this.MainVM.Translate(); 

            this.MainVM.MainWindow.Show(); 
            this.Close(); 
        }

        private void LoginView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!IsLoggedIn)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }
    }
}

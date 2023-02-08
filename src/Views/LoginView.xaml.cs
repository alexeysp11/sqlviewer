using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;
using SqlViewer.Entities.ViewsEntities; 
using SqlViewer.Helpers; 
using LanguageEnum = SqlViewer.Enums.Common.Language; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }
        
        /// <summary>
        /// Entity of the view, that is used for translating visual elements 
        /// </summary>
        private LoginEntity LoginEntity { get; set; }

        /// <summary>
        /// Boolean variable for getting if user logged on 
        /// </summary>
        private bool IsLoggedIn { get; set; } = false; 

        /// <summary>
        /// Constructor of LoginView
        /// </summary>
        public LoginView()
        {
            InitializeComponent();

            Loaded += (o, e) => 
            {
                this.MainVM = ((MainVM)this.DataContext); 
                this.LoginEntity = this.MainVM.Translator.LoginEntity; 
                this.MainVM.VisualVM.LoginView = this; 
                Init(); 
            }; 
        }

        /// <summary>
        /// General method that initializes the view 
        /// </summary>
        private void Init()
        {
            try
            {
                this.MainVM.InitAppRepository(); 
                this.MainVM.Translate(); 

                InitPreferencesDb();
                InitButtons();
                InitDbCredentials(); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Initializes section of database preferences 
        /// </summary>
        private void InitPreferencesDb()
        {
            lblActiveRdbms.Content = SettingsHelper.TranslateUiElement(LoginEntity.ActiveRdbmsField.English, LoginEntity.ActiveRdbmsField.Translation); 
            cbActiveRdbms.Text = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.ActiveRdbms.ToString()) ? RdbmsEnum.SQLite.ToString() : RepoHelper.AppSettingsRepo.ActiveRdbms.ToString(); 

            lblDatabase.Content = SettingsHelper.TranslateUiElement(LoginEntity.DatabaseField.English, LoginEntity.DatabaseField.Translation); 
            lblSchema.Content = SettingsHelper.TranslateUiElement(LoginEntity.SchemaField.English, LoginEntity.SchemaField.Translation); 
            lblUsername.Content = SettingsHelper.TranslateUiElement(LoginEntity.UsernameField.English, LoginEntity.UsernameField.Translation); 
        }

        /// <summary>
        /// Initializes section of buttons 
        /// </summary>
        private void InitButtons()
        {
            btnLogIn.Content = SettingsHelper.TranslateUiElement(LoginEntity.LogInField.English, LoginEntity.LogInField.Translation); 
            btnCancel.Content = SettingsHelper.TranslateUiElement(LoginEntity.CancelField.English, LoginEntity.CancelField.Translation); 
        }
        
        /// <summary>
        /// Initializes section of entering data about preferable database connection 
        /// </summary>
        private void InitDbCredentials()
        {
            if (cbActiveRdbms.Text == "SQLite")
            {
                tbServer.IsEnabled = false; 
                tbPort.IsEnabled = false; 
                tbSchema.IsEnabled = false; 
                tbUsername.IsEnabled = false; 
                pbPassword.IsEnabled = false; 
                btnDatabase.IsEnabled = true; 

                tbServer.Background = System.Windows.Media.Brushes.Gray; 
                tbPort.Background = System.Windows.Media.Brushes.Gray; 
                tbSchema.Background = System.Windows.Media.Brushes.Gray; 
                tbUsername.Background = System.Windows.Media.Brushes.Gray; 
                pbPassword.Background = System.Windows.Media.Brushes.Gray; 
            }
            else
            {
                tbServer.IsEnabled = true; 
                tbPort.IsEnabled = true; 
                tbSchema.IsEnabled = true; 
                tbUsername.IsEnabled = true; 
                pbPassword.IsEnabled = true;
                btnDatabase.IsEnabled = false; 

                tbServer.Background = System.Windows.Media.Brushes.White; 
                tbPort.Background = System.Windows.Media.Brushes.White; 
                tbSchema.Background = System.Windows.Media.Brushes.White; 
                tbUsername.Background = System.Windows.Media.Brushes.White; 
                pbPassword.Background = System.Windows.Media.Brushes.White; 
            }
            tbServer.Text = System.String.Empty; 
            tbDatabase.Text = System.String.Empty; 
            tbPort.Text = System.String.Empty; 
            tbSchema.Text = System.String.Empty; 
            tbUsername.Text = System.String.Empty; 
            pbPassword.Password = System.String.Empty; 

            RepoHelper.AppSettingsRepo.SetActiveRdbms(cbActiveRdbms.Text); 
        }

        /// <summary>
        /// Handles click on the LogIn button 
        /// </summary>
        private void btnLogIn_Clicked(object sender, RoutedEventArgs e)
        {
            this.IsLoggedIn = true; 

            string sql = this.MainVM.DataVM.MainDbBranch.GetSqlRequest("Sqlite/App/UpdateSettingsDb.sql"); 
            sql = string.Format(sql, "SQLite", cbActiveRdbms.Text, tbServer.Text, tbDatabase.Text, tbPort.Text, tbSchema.Text, tbUsername.Text, pbPassword.Password); 
            this.MainVM.DataVM.MainDbBranch.SendSqlRequest(sql); 
            this.MainVM.InitAppRepository(); 
            this.MainVM.Translate(); 

            this.MainVM.VisualVM.LoginView = null; 
            this.MainVM.MainWindow.Show(); 
            this.Close(); 
        }

        /// <summary>
        /// Handles selection of active RDBMS 
        /// </summary>
        private void cbActiveRdbms_DropDownClosed(object sender, System.EventArgs e)
        {
            InitDbCredentials(); 
        }

        /// <summary>
        /// Exits the application, if user is not logged in 
        /// </summary>
        private void LoginView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!IsLoggedIn)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }
    }
}

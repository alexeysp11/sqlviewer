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
        private MainVM MainVM { get; set; }
        private LoginEntity LoginEntity { get; set; }

        private bool IsLoggedIn { get; set; } = false; 

        public LoginView()
        {
            InitializeComponent();

            Loaded += (o, e) => 
            {
                this.MainVM = ((MainVM)this.DataContext); 
                this.LoginEntity = this.MainVM.Translator.LoginEntity; 
                Init(); 
            }; 
        }

        private void Init()
        {
            try
            {
                this.MainVM.InitAppRepository(); 
                this.MainVM.TranslateLogin(); 

                InitPreferencesDb();
                InitButtons();
                InitDbCredentials(); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitPreferencesDb()
        {
            lblActiveRdbms.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(LoginEntity.ActiveRdbmsField.Translation) ? LoginEntity.ActiveRdbmsField.English : LoginEntity.ActiveRdbmsField.Translation; 
            cbActiveRdbms.Text = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.ActiveRdbms.ToString()) ? RdbmsEnum.SQLite.ToString() : RepoHelper.AppSettingsRepo.ActiveRdbms.ToString(); 

            lblDatabase.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(LoginEntity.DatabaseField.Translation) ? LoginEntity.DatabaseField.English : LoginEntity.DatabaseField.Translation; 
            lblSchema.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(LoginEntity.SchemaField.Translation) ? LoginEntity.SchemaField.English : LoginEntity.SchemaField.Translation; 
            lblUsername.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(LoginEntity.UsernameField.Translation) ? LoginEntity.UsernameField.English : LoginEntity.UsernameField.Translation; 
            lblPassword.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(LoginEntity.PasswordField.Translation) ? LoginEntity.PasswordField.English : LoginEntity.PasswordField.Translation; 
        }

        private void InitButtons()
        {
            btnLogIn.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(LoginEntity.LogInField.Translation) ? LoginEntity.LogInField.English : LoginEntity.LogInField.Translation; 
            btnCancel.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(LoginEntity.CancelField.Translation) ? LoginEntity.CancelField.English : LoginEntity.CancelField.Translation; 
        }
        
        private void InitDbCredentials()
        {
            if (cbActiveRdbms.Text == "SQLite")
            {
                tbSchema.IsEnabled = false; 
                tbUsername.IsEnabled = false; 
                pbPassword.IsEnabled = false; 

                tbSchema.Text = System.String.Empty; 
                tbUsername.Text = System.String.Empty; 
                pbPassword.Password = System.String.Empty; 

                tbSchema.Background = System.Windows.Media.Brushes.Gray; 
                tbUsername.Background = System.Windows.Media.Brushes.Gray; 
                pbPassword.Background = System.Windows.Media.Brushes.Gray; 
            }
            else
            { 
                tbSchema.IsEnabled = true; 
                tbUsername.IsEnabled = true; 
                pbPassword.IsEnabled = true;

                tbSchema.Background = System.Windows.Media.Brushes.White; 
                tbUsername.Background = System.Windows.Media.Brushes.White; 
                pbPassword.Background = System.Windows.Media.Brushes.White; 
            }
        }

        private void btnLogIn_Clicked(object sender, RoutedEventArgs e)
        {
            this.IsLoggedIn = true; 

            string sql = this.MainVM.DbVM.GetSqlRequest("App/UpdateSettingsDb.sql"); 
            sql = string.Format(sql, "SQLite", cbActiveRdbms.Text, tbDatabase.Text, tbSchema.Text, tbUsername.Text, pbPassword.Password); 
            this.MainVM.DbVM.SendSqlRequest(sql); 
            this.MainVM.InitAppRepository(); 
            this.MainVM.Translate(); 

            this.MainVM.MainWindow.Show(); 
            this.Close(); 
        }

        private void cbActiveRdbms_DropDownClosed(object sender, System.EventArgs e)
        {
            InitDbCredentials(); 
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

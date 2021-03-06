using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;
using SqlViewer.Entities.ViewsEntities; 
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
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitPreferencesDb()
        {
            lblActiveRdbms.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(LoginEntity.ActiveRdbmsField.Translation) ? LoginEntity.ActiveRdbmsField.English : LoginEntity.ActiveRdbmsField.Translation; 
            cbActiveRdbms.Text = System.String.IsNullOrEmpty(this.MainVM.AppRepository.ActiveRdbms.ToString()) ? RdbmsEnum.SQLite.ToString() : this.MainVM.AppRepository.ActiveRdbms.ToString(); 

            lblDatabase.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(LoginEntity.DatabaseField.Translation) ? LoginEntity.DatabaseField.English : LoginEntity.DatabaseField.Translation; 
            lblSchema.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(LoginEntity.SchemaField.Translation) ? LoginEntity.SchemaField.English : LoginEntity.SchemaField.Translation; 
            lblUsername.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(LoginEntity.UsernameField.Translation) ? LoginEntity.UsernameField.English : LoginEntity.UsernameField.Translation; 
            lblPassword.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(LoginEntity.PasswordField.Translation) ? LoginEntity.PasswordField.English : LoginEntity.PasswordField.Translation; 
        }

        private void InitButtons()
        {
            btnLogIn.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(LoginEntity.LogInField.Translation) ? LoginEntity.LogInField.English : LoginEntity.LogInField.Translation; 
            btnCancel.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(LoginEntity.CancelField.Translation) ? LoginEntity.CancelField.English : LoginEntity.CancelField.Translation; 
        }

        private void btnLogIn_Clicked(object sender, RoutedEventArgs e)
        {
            this.IsLoggedIn = true; 

            System.String sql = this.MainVM.GetSqlRequest("App/UpdateSettingsDb.sql"); 
            sql = System.String.Format(sql, "SQLite", cbActiveRdbms.Text, tbDatabase.Text, 
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

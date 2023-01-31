using System.Collections.Generic;
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
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        /// <summary>
        /// 
        /// </summary>
        private MainVM MainVM { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        private SettingsEntity SettingsEntity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private string OldActiveRdbms { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SettingsView()
        {
            InitializeComponent();

            Loaded += (o, e) => 
            {
                this.MainVM = (MainVM)this.DataContext; 
                this.MainVM.VisualVM.SettingsView = this; 
                this.SettingsEntity = this.MainVM.Translator.SettingsEntity;  
                Init(); 
                OldActiveRdbms = cbActiveRdbms.Text; 
            }; 
        }

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            InitPreferencesEditor(); 
            InitPreferencesDb();
            InitButtons();
            InitDbCredentials(); 
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitPreferencesEditor()
        {
            lblPreferencesEditor.Content = SettingsHelper.TranslateUiElement(SettingsEntity.EditorField.English, SettingsEntity.EditorField.Translation); 

            lblLanguage.Content = SettingsHelper.TranslateUiElement(SettingsEntity.LanguageField.English, SettingsEntity.LanguageField.Translation); 
            cbLanguage.Text = RepoHelper.AppSettingsRepo.Language.ToString();

            lblAutoSave.Content = SettingsHelper.TranslateUiElement(SettingsEntity.AutoSaveField.English, SettingsEntity.AutoSaveField.Translation); 
            cbAutoSave.Text = RepoHelper.AppSettingsRepo.AutoSave.ToString();
            cbiAutoSaveEnabled.Content = SettingsHelper.TranslateUiElement(SettingsEntity.EnabledField.English, SettingsEntity.EnabledField.Translation); 
            cbiAutoSaveDisabled.Content = SettingsHelper.TranslateUiElement(SettingsEntity.DisabledField.English, SettingsEntity.DisabledField.Translation); 

            lblFontSize.Content = SettingsHelper.TranslateUiElement(SettingsEntity.FontSizeField.English, SettingsEntity.FontSizeField.Translation); 
            cbFontSize.Text = RepoHelper.EnumDecoder.GetFontSizeName(RepoHelper.AppSettingsRepo.FontSize); 
            lblFontFamily.Content = SettingsHelper.TranslateUiElement(SettingsEntity.FontFamilyField.English, SettingsEntity.FontFamilyField.Translation); 
            cbFontFamily.Text = RepoHelper.AppSettingsRepo.FontFamily.ToString(); 
            lblTabSize.Content = SettingsHelper.TranslateUiElement(SettingsEntity.TabSizeField.English, SettingsEntity.TabSizeField.Translation); 
            cbTabSize.Text = RepoHelper.EnumDecoder.GetTabSizeName(RepoHelper.AppSettingsRepo.TabSize); 

            lblWordWrap.Content = SettingsHelper.TranslateUiElement(SettingsEntity.WordWrapField.English, SettingsEntity.WordWrapField.Translation); 
            cbWordWrap.Text = RepoHelper.AppSettingsRepo.WordWrap.ToString(); 
            cbiWordWrapEnabled.Content = SettingsHelper.TranslateUiElement(SettingsEntity.EnabledField.English, SettingsEntity.EnabledField.Translation); 
            cbiWordWrapDisabled.Content = SettingsHelper.TranslateUiElement(SettingsEntity.DisabledField.English, SettingsEntity.DisabledField.Translation); 
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitPreferencesDb()
        {
            lblPreferencesDb.Content = SettingsHelper.TranslateUiElement(SettingsEntity.DbField.English, SettingsEntity.DbField.Translation); 
            lblDefaultRdbms.Content = SettingsHelper.TranslateUiElement(SettingsEntity.DefaultRdbmsField.English, SettingsEntity.DefaultRdbmsField.Translation); 
            cbDefaultRdbms.Text = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.ActiveRdbms.ToString()) ? RdbmsEnum.SQLite.ToString() : RepoHelper.AppSettingsRepo.ActiveRdbms.ToString(); 
            lblActiveRdbms.Content = SettingsHelper.TranslateUiElement(SettingsEntity.ActiveRdbmsField.English, SettingsEntity.ActiveRdbmsField.Translation); 
            cbActiveRdbms.Text = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.ActiveRdbms.ToString()) ? RdbmsEnum.SQLite.ToString() : RepoHelper.AppSettingsRepo.ActiveRdbms.ToString(); 

            lblDatabase.Content = SettingsHelper.TranslateUiElement(SettingsEntity.DatabaseField.English, SettingsEntity.DatabaseField.Translation); 
            lblSchema.Content = SettingsHelper.TranslateUiElement(SettingsEntity.SchemaField.English, SettingsEntity.SchemaField.Translation); 
            lblUsername.Content = SettingsHelper.TranslateUiElement(SettingsEntity.UsernameField.English, SettingsEntity.UsernameField.Translation); 
            lblPassword.Content = SettingsHelper.TranslateUiElement(SettingsEntity.PasswordField.English, SettingsEntity.PasswordField.Translation); 
            
            tbDatabase.Text = RepoHelper.AppSettingsRepo.DbName; 
            tbSchema.Text = RepoHelper.AppSettingsRepo.DbSchema; 
            tbUsername.Text = RepoHelper.AppSettingsRepo.DbUsername; 
            pbPassword.Password = RepoHelper.AppSettingsRepo.DbPassword; 
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitButtons()
        {
            btnRecover.Content = SettingsHelper.TranslateUiElement(SettingsEntity.RecoverField.English, SettingsEntity.RecoverField.Translation); 
            btnSave.Content = SettingsHelper.TranslateUiElement(SettingsEntity.SaveField.English, SettingsEntity.SaveField.Translation); 
            btnCancel.Content = SettingsHelper.TranslateUiElement(SettingsEntity.CancelField.English, SettingsEntity.CancelField.Translation); 
        }

        /// <summary>
        /// 
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
        /// Allows to update settings repository (it is invoked from MainVM)
        /// </summary>
        public void UpdateAppRepository()
        {
            try 
            {
                RepoHelper.AppSettingsRepo.SetLanguage(cbLanguage.Text); 
                RepoHelper.AppSettingsRepo.SetAutoSave(cbAutoSave.Text); 
                RepoHelper.AppSettingsRepo.SetFontSize(System.Convert.ToInt32(cbFontSize.Text)); 
                RepoHelper.AppSettingsRepo.SetFontFamily(cbFontFamily.Text); 
                RepoHelper.AppSettingsRepo.SetTabSize(System.Convert.ToInt32(cbTabSize.Text)); 
                RepoHelper.AppSettingsRepo.SetWordWrap(cbWordWrap.Text); 
                RepoHelper.AppSettingsRepo.SetDefaultRdbms(cbDefaultRdbms.Text); 
                RepoHelper.AppSettingsRepo.SetActiveRdbms(cbActiveRdbms.Text); 
                RepoHelper.AppSettingsRepo.SetDbHost(tbServer.Text); 
                RepoHelper.AppSettingsRepo.SetDbName(tbDatabase.Text); 
                RepoHelper.AppSettingsRepo.SetDbPort(tbPort.Text); 
                RepoHelper.AppSettingsRepo.SetDbSchema(tbSchema.Text); 
                RepoHelper.AppSettingsRepo.SetDbUsername(tbUsername.Text); 
                RepoHelper.AppSettingsRepo.SetDbPassword(pbPassword.Password); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CancelChangesAppRepository()
        {
            string activeRdbms = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.ActiveRdbms.ToString()) ? RdbmsEnum.SQLite.ToString() : RepoHelper.AppSettingsRepo.ActiveRdbms.ToString(); 
            if (activeRdbms != OldActiveRdbms)
            {
                RepoHelper.AppSettingsRepo.SetActiveRdbms(OldActiveRdbms); 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        private void SettingsView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainVM)this.DataContext).VisualVM.SettingsView = null; 
        }

        /// <summary>
        /// 
        /// </summary>
        private void cbActiveRdbms_DropDownClosed(object sender, System.EventArgs e)
        {
            InitDbCredentials(); 
        }
    }
}

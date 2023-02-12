using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SqlViewer.Entities.ViewsEntities; 
using SqlViewer.Helpers; 
using SqlViewer.Models.DataStorage; 
using SqlViewer.ViewModels;
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
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }
        
        /// <summary>
        /// Entity of the view, that is used for translating visual elements 
        /// </summary>
        private SettingsEntity SettingsEntity { get; set; }

        /// <summary>
        /// String variable for storing previously selected Active RDBMS 
        /// </summary>
        private string OldActiveRdbms { get; set; }

        /// <summary>
        /// Constructor of SettingsView
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
        /// General method that initializes the view 
        /// </summary>
        public void Init()
        {
            InitPreferencesEditor(); 
            InitPreferencesDb();
            InitButtons();
            InitDbCredentials(); 
        }

        /// <summary>
        /// Initializes section of editor preferences 
        /// </summary>
        private void InitPreferencesEditor()
        {
            lblPreferencesEditor.Content = SettingsHelper.TranslateUiElement(SettingsEntity.EditorField.English, SettingsEntity.EditorField.Translation); 

            lblLanguage.Content = SettingsHelper.TranslateUiElement(SettingsEntity.LanguageField.English, SettingsEntity.LanguageField.Translation); 
            cbLanguage.Text = RepoHelper.AppSettingsRepo.Language.ToString();

            lblAutoSave.Content = SettingsHelper.TranslateUiElement(SettingsEntity.AutoSaveField.English, SettingsEntity.AutoSaveField.Translation); 
            cbAutoSave.Text = RepoHelper.AppSettingsRepo.EditorSettings.AutoSave.ToString();
            cbiAutoSaveEnabled.Content = SettingsHelper.TranslateUiElement(SettingsEntity.EnabledField.English, SettingsEntity.EnabledField.Translation); 
            cbiAutoSaveDisabled.Content = SettingsHelper.TranslateUiElement(SettingsEntity.DisabledField.English, SettingsEntity.DisabledField.Translation); 

            lblFontSize.Content = SettingsHelper.TranslateUiElement(SettingsEntity.FontSizeField.English, SettingsEntity.FontSizeField.Translation); 
            cbFontSize.Text = RepoHelper.EnumDecoder.GetFontSizeName(RepoHelper.AppSettingsRepo.EditorSettings.FontSize); 
            lblFontFamily.Content = SettingsHelper.TranslateUiElement(SettingsEntity.FontFamilyField.English, SettingsEntity.FontFamilyField.Translation); 
            cbFontFamily.Text = RepoHelper.AppSettingsRepo.EditorSettings.FontFamily.ToString(); 
            lblTabSize.Content = SettingsHelper.TranslateUiElement(SettingsEntity.TabSizeField.English, SettingsEntity.TabSizeField.Translation); 
            cbTabSize.Text = RepoHelper.EnumDecoder.GetTabSizeName(RepoHelper.AppSettingsRepo.EditorSettings.TabSize); 

            lblWordWrap.Content = SettingsHelper.TranslateUiElement(SettingsEntity.WordWrapField.English, SettingsEntity.WordWrapField.Translation); 
            cbWordWrap.Text = RepoHelper.AppSettingsRepo.EditorSettings.WordWrap.ToString(); 
            cbiWordWrapEnabled.Content = SettingsHelper.TranslateUiElement(SettingsEntity.EnabledField.English, SettingsEntity.EnabledField.Translation); 
            cbiWordWrapDisabled.Content = SettingsHelper.TranslateUiElement(SettingsEntity.DisabledField.English, SettingsEntity.DisabledField.Translation); 
        }

        /// <summary>
        /// Initializes section of database preferences 
        /// </summary>
        private void InitPreferencesDb()
        {
            lblPreferencesDb.Content = SettingsHelper.TranslateUiElement(SettingsEntity.DbField.English, SettingsEntity.DbField.Translation); 
            lblDefaultRdbms.Content = SettingsHelper.TranslateUiElement(SettingsEntity.DefaultRdbmsField.English, SettingsEntity.DefaultRdbmsField.Translation); 
            cbDefaultRdbms.Text = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms.ToString()) ? RdbmsEnum.SQLite.ToString() : RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms.ToString(); 
            lblActiveRdbms.Content = SettingsHelper.TranslateUiElement(SettingsEntity.ActiveRdbmsField.English, SettingsEntity.ActiveRdbmsField.Translation); 
            cbActiveRdbms.Text = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms.ToString()) ? RdbmsEnum.SQLite.ToString() : RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms.ToString(); 

            lblDatabase.Content = SettingsHelper.TranslateUiElement(SettingsEntity.DatabaseField.English, SettingsEntity.DatabaseField.Translation); 
            lblSchema.Content = SettingsHelper.TranslateUiElement(SettingsEntity.SchemaField.English, SettingsEntity.SchemaField.Translation); 
            lblUsername.Content = SettingsHelper.TranslateUiElement(SettingsEntity.UsernameField.English, SettingsEntity.UsernameField.Translation); 
            lblPassword.Content = SettingsHelper.TranslateUiElement(SettingsEntity.PasswordField.English, SettingsEntity.PasswordField.Translation); 
            
            tbDatabase.Text = RepoHelper.AppSettingsRepo.DatabaseSettings.DbName; 
            tbSchema.Text = RepoHelper.AppSettingsRepo.DatabaseSettings.DbSchema; 
            tbUsername.Text = RepoHelper.AppSettingsRepo.DatabaseSettings.DbUsername; 
            pbPassword.Password = RepoHelper.AppSettingsRepo.DatabaseSettings.DbPassword; 
        }

        /// <summary>
        /// Initializes section of buttons 
        /// </summary>
        private void InitButtons()
        {
            btnRecover.Content = SettingsHelper.TranslateUiElement(SettingsEntity.RecoverField.English, SettingsEntity.RecoverField.Translation); 
            btnSave.Content = SettingsHelper.TranslateUiElement(SettingsEntity.SaveField.English, SettingsEntity.SaveField.Translation); 
            btnCancel.Content = SettingsHelper.TranslateUiElement(SettingsEntity.CancelField.English, SettingsEntity.CancelField.Translation); 
        }

        /// <summary>
        /// Initializes section of entering data about preferable database connection 
        /// </summary>
        private void InitDbCredentials()
        {
            DbCredentialsVE dbCredentialsVE = new DbCredentialsVE()
            {
                cbActiveRdbms = this.cbActiveRdbms, 
                tbServer = this.tbServer, 
                tbDatabase = this.tbDatabase, 
                tbPort = this.tbPort, 
                tbSchema = this.tbSchema, 
                tbUsername = this.tbUsername, 
                pbPassword = this.pbPassword, 
                btnDatabase = this.btnDatabase
            };
            MainVM.VisualVM.InitDbCredentials(dbCredentialsVE); 
        }

        /// <summary>
        /// Allows to update settings repository (it is invoked from MainVM)
        /// </summary>
        public void UpdateAppRepository()
        {
            try 
            {
                RepoHelper.AppSettingsRepo.SetLanguage(cbLanguage.Text); 
                RepoHelper.AppSettingsRepo.EditorSettings.SetAutoSave(cbAutoSave.Text); 
                RepoHelper.AppSettingsRepo.EditorSettings.SetFontSize(System.Convert.ToInt32(cbFontSize.Text)); 
                RepoHelper.AppSettingsRepo.EditorSettings.SetFontFamily(cbFontFamily.Text); 
                RepoHelper.AppSettingsRepo.EditorSettings.SetTabSize(System.Convert.ToInt32(cbTabSize.Text)); 
                RepoHelper.AppSettingsRepo.EditorSettings.SetWordWrap(cbWordWrap.Text); 
                RepoHelper.AppSettingsRepo.DatabaseSettings.SetDefaultRdbms(cbDefaultRdbms.Text); 
                RepoHelper.AppSettingsRepo.DatabaseSettings.SetActiveRdbms(cbActiveRdbms.Text); 
                RepoHelper.AppSettingsRepo.DatabaseSettings.SetDbHost(tbServer.Text); 
                RepoHelper.AppSettingsRepo.DatabaseSettings.SetDbName(tbDatabase.Text); 
                RepoHelper.AppSettingsRepo.DatabaseSettings.SetDbPort(tbPort.Text); 
                RepoHelper.AppSettingsRepo.DatabaseSettings.SetDbSchema(tbSchema.Text); 
                RepoHelper.AppSettingsRepo.DatabaseSettings.SetDbUsername(tbUsername.Text); 
                RepoHelper.AppSettingsRepo.DatabaseSettings.SetDbPassword(pbPassword.Password); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        /// <summary>
        /// Cancels changes that are made on the view (is invoked from MainVM.CancelSettings() method)
        /// </summary>
        public void CancelChangesAppRepository()
        {
            string activeRdbms = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms.ToString()) ? RdbmsEnum.SQLite.ToString() : RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms.ToString(); 
            if (activeRdbms != OldActiveRdbms)
            {
                RepoHelper.AppSettingsRepo.DatabaseSettings.SetActiveRdbms(OldActiveRdbms); 
            } 
        }

        /// <summary>
        /// Handles selection of active RDBMS 
        /// </summary>
        private void cbActiveRdbms_DropDownClosed(object sender, System.EventArgs e)
        {
            InitDbCredentials(); 
        }

        /// <summary>
        /// Event handler which sets the reference to itself in MainVM to null, when the view is closing 
        /// </summary>
        private void SettingsView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainVM)this.DataContext).VisualVM.SettingsView = null; 
        }
    }
}

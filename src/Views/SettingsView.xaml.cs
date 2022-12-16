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
        private MainVM MainVM { get; set; }
        private SettingsEntity SettingsEntity { get; set; }

        public SettingsView()
        {
            InitializeComponent();

            Loaded += (o, e) => 
            {
                this.MainVM = (MainVM)this.DataContext; 
                this.MainVM.VisualVM.SettingsView = this; 
                this.SettingsEntity = this.MainVM.Translator.SettingsEntity;  
                Init(); 
            }; 
        }

        public void Init()
        {
            InitPreferencesEditor(); 
            InitPreferencesDb();
            InitButtons();
            InitDbCredentials(); 
        }

        private void InitPreferencesEditor()
        {
            lblPreferencesEditor.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.EditorField.Translation) ? SettingsEntity.EditorField.English : SettingsEntity.EditorField.Translation; 

            lblLanguage.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.LanguageField.Translation) ? SettingsEntity.LanguageField.English : SettingsEntity.LanguageField.Translation; 
            cbLanguage.Text = RepoHelper.AppSettingsRepo.Language.ToString();
            cbiLanguageEnglish.Content = "English"; 
            cbiLanguageGerman.Content = "German"; 
            cbiLanguageRussian.Content = "Russian"; 
            cbiLanguageSpanish.Content = "Spanish"; 
            cbiLanguagePortuguese.Content = "Portuguese"; 
            cbiLanguageItalian.Content = "Italian"; 
            cbiLanguageFrench.Content = "French"; 
            cbiLanguageUkranian.Content = "Ukranian"; 
            cbiLanguageDutch.Content = "Dutch"; 

            lblAutoSave.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.AutoSaveField.Translation) ? SettingsEntity.AutoSaveField.English : SettingsEntity.AutoSaveField.Translation; 
            cbAutoSave.Text = RepoHelper.AppSettingsRepo.AutoSave.ToString();
            cbiAutoSaveEnabled.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.EnabledField.Translation) ? SettingsEntity.EnabledField.English : SettingsEntity.EnabledField.Translation; 
            cbiAutoSaveDisabled.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.DisabledField.Translation) ? SettingsEntity.DisabledField.English : SettingsEntity.DisabledField.Translation; 

            lblFontSize.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.FontSizeField.Translation) ? SettingsEntity.FontSizeField.English : SettingsEntity.FontSizeField.Translation; 
            cbFontSize.Text = EnumCodecHelper.EnumDecoder.GetFontSizeName(RepoHelper.AppSettingsRepo.FontSize); 
            lblFontFamily.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.FontFamilyField.Translation) ? SettingsEntity.FontFamilyField.English : SettingsEntity.FontFamilyField.Translation; 
            cbFontFamily.Text = RepoHelper.AppSettingsRepo.FontFamily.ToString(); 
            lblTabSize.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.TabSizeField.Translation) ? SettingsEntity.TabSizeField.English : SettingsEntity.TabSizeField.Translation; 
            cbTabSize.Text = EnumCodecHelper.EnumDecoder.GetTabSizeName(RepoHelper.AppSettingsRepo.TabSize); 

            lblWordWrap.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.WordWrapField.Translation) ? SettingsEntity.WordWrapField.English : SettingsEntity.WordWrapField.Translation; 
            cbWordWrap.Text = RepoHelper.AppSettingsRepo.WordWrap.ToString(); 
            cbiWordWrapEnabled.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.EnabledField.Translation) ? SettingsEntity.EnabledField.English : SettingsEntity.EnabledField.Translation; 
            cbiWordWrapDisabled.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.DisabledField.Translation) ? SettingsEntity.DisabledField.English : SettingsEntity.DisabledField.Translation; 
        }

        private void InitPreferencesDb()
        {
            lblPreferencesDb.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.DbField.Translation) ? SettingsEntity.DbField.English : SettingsEntity.DbField.Translation; 
            lblDefaultRdbms.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.DefaultRdbmsField.Translation) ? SettingsEntity.DefaultRdbmsField.English : SettingsEntity.DefaultRdbmsField.Translation; 
            cbDefaultRdbms.Text = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.ActiveRdbms.ToString()) ? RdbmsEnum.SQLite.ToString() : RepoHelper.AppSettingsRepo.ActiveRdbms.ToString(); 
            lblActiveRdbms.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.ActiveRdbmsField.Translation) ? SettingsEntity.ActiveRdbmsField.English : SettingsEntity.ActiveRdbmsField.Translation; 
            cbActiveRdbms.Text = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.ActiveRdbms.ToString()) ? RdbmsEnum.SQLite.ToString() : RepoHelper.AppSettingsRepo.ActiveRdbms.ToString(); 

            lblDatabase.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.DatabaseField.Translation) ? SettingsEntity.DatabaseField.English : SettingsEntity.DatabaseField.Translation; 
            lblSchema.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.SchemaField.Translation) ? SettingsEntity.SchemaField.English : SettingsEntity.SchemaField.Translation; 
            lblUsername.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.UsernameField.Translation) ? SettingsEntity.UsernameField.English : SettingsEntity.UsernameField.Translation; 
            lblPassword.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.PasswordField.Translation) ? SettingsEntity.PasswordField.English : SettingsEntity.PasswordField.Translation; 
            
            tbDatabase.Text = RepoHelper.AppSettingsRepo.DbName; 
            tbSchema.Text = RepoHelper.AppSettingsRepo.DbSchema; 
            tbUsername.Text = RepoHelper.AppSettingsRepo.DbUsername; 
            pbPassword.Password = RepoHelper.AppSettingsRepo.DbPassword; 
        }

        private void InitButtons()
        {
            btnRecover.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.RecoverField.Translation) ? SettingsEntity.RecoverField.English : SettingsEntity.RecoverField.Translation; 
            btnSave.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.SaveField.Translation) ? SettingsEntity.SaveField.English : SettingsEntity.SaveField.Translation; 
            btnCancel.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SettingsEntity.CancelField.Translation) ? SettingsEntity.CancelField.English : SettingsEntity.CancelField.Translation; 
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

        public void UpdateAppRepository()
        {
            try 
            {
                RepoHelper.AppSettingsRepo.Update(cbLanguage.Text, cbAutoSave.Text, System.Convert.ToInt32(cbFontSize.Text), 
                    cbFontFamily.Text, System.Convert.ToInt32(cbTabSize.Text), cbWordWrap.Text, cbDefaultRdbms.Text, 
                    cbActiveRdbms.Text, tbDatabase.Text, tbSchema.Text, tbUsername.Text, pbPassword.Password); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        private void SettingsView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainVM)this.DataContext).VisualVM.SettingsView = null; 
        }

        private void cbActiveRdbms_DropDownClosed(object sender, System.EventArgs e)
        {
            InitDbCredentials(); 
        }
    }
}

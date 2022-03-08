using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;
using SqlViewer.Entities.ViewsEntities; 
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
                this.MainVM.SettingsView = this; 
                this.SettingsEntity = this.MainVM.Translator.SettingsEntity;  
                Init(); 
            }; 
        }

        public void Init()
        {
            InitPreferencesEditor(); 
            InitPreferencesDb();
            InitButtons();
        }

        private void InitPreferencesEditor()
        {
            lblPreferencesEditor.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.EditorField.Translation) ? SettingsEntity.EditorField.English : SettingsEntity.EditorField.Translation; 

            lblLanguage.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.LanguageField.Translation) ? SettingsEntity.LanguageField.English : SettingsEntity.LanguageField.Translation; 
            cbLanguage.Text = this.MainVM.AppRepository.Language.ToString();
            cbiLanguageEnglish.Content = "English"; 
            cbiLanguageGerman.Content = "German"; 
            cbiLanguageRussian.Content = "Russian"; 
            cbiLanguageSpanish.Content = "Spanish"; 
            cbiLanguagePortugues.Content = "Portugues"; 
            cbiLanguageItalian.Content = "Italian"; 
            cbiLanguageFrench.Content = "French"; 
            cbiLanguageUkranian.Content = "Ukranian"; 
            cbiLanguageDutch.Content = "Dutch"; 

            lblAutoSave.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.AutoSaveField.Translation) ? SettingsEntity.AutoSaveField.English : SettingsEntity.AutoSaveField.Translation; 
            cbAutoSave.Text = this.MainVM.AppRepository.AutoSave.ToString();
            cbiAutoSaveEnabled.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.EnabledField.Translation) ? SettingsEntity.EnabledField.English : SettingsEntity.EnabledField.Translation; 
            cbiAutoSaveDisabled.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.DisabledField.Translation) ? SettingsEntity.DisabledField.English : SettingsEntity.DisabledField.Translation; 

            lblFontSize.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.FontSizeField.Translation) ? SettingsEntity.FontSizeField.English : SettingsEntity.FontSizeField.Translation; 
            cbFontSize.Text = this.MainVM.EnumDecoder.GetFontSizeName(this.MainVM.AppRepository.FontSize); 
            lblFontFamily.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.FontFamilyField.Translation) ? SettingsEntity.FontFamilyField.English : SettingsEntity.FontFamilyField.Translation; 
            cbFontFamily.Text = this.MainVM.AppRepository.FontFamily.ToString(); 
            lblTabSize.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.TabSizeField.Translation) ? SettingsEntity.TabSizeField.English : SettingsEntity.TabSizeField.Translation; 
            cbTabSize.Text = this.MainVM.EnumDecoder.GetTabSizeName(this.MainVM.AppRepository.TabSize); 

            lblWordWrap.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.WordWrapField.Translation) ? SettingsEntity.WordWrapField.English : SettingsEntity.WordWrapField.Translation; 
            cbWordWrap.Text = this.MainVM.AppRepository.WordWrap.ToString(); 
            cbiWordWrapEnabled.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.EnabledField.Translation) ? SettingsEntity.EnabledField.English : SettingsEntity.EnabledField.Translation; 
            cbiWordWrapDisabled.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.DisabledField.Translation) ? SettingsEntity.DisabledField.English : SettingsEntity.DisabledField.Translation; 
        }

        private void InitPreferencesDb()
        {
            lblPreferencesDb.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.DbField.Translation) ? SettingsEntity.DbField.English : SettingsEntity.DbField.Translation; 
            lblDefaultRdbms.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.DefaultRdbmsField.Translation) ? SettingsEntity.DefaultRdbmsField.English : SettingsEntity.DefaultRdbmsField.Translation; 
            cbDefaultRdbms.Text = System.String.IsNullOrEmpty(this.MainVM.AppRepository.DefaultRdbms.ToString()) ? RdbmsEnum.SQLite.ToString() : this.MainVM.AppRepository.DefaultRdbms.ToString(); 
            lblActiveRdbms.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.ActiveRdbmsField.Translation) ? SettingsEntity.ActiveRdbmsField.English : SettingsEntity.ActiveRdbmsField.Translation; 
            cbActiveRdbms.Text = System.String.IsNullOrEmpty(this.MainVM.AppRepository.ActiveRdbms.ToString()) ? RdbmsEnum.SQLite.ToString() : this.MainVM.AppRepository.ActiveRdbms.ToString(); 

            lblDatabase.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.DatabaseField.Translation) ? SettingsEntity.DatabaseField.English : SettingsEntity.DatabaseField.Translation; 
            lblSchema.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.SchemaField.Translation) ? SettingsEntity.SchemaField.English : SettingsEntity.SchemaField.Translation; 
            lblUsername.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.UsernameField.Translation) ? SettingsEntity.UsernameField.English : SettingsEntity.UsernameField.Translation; 
            lblPassword.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.PasswordField.Translation) ? SettingsEntity.PasswordField.English : SettingsEntity.PasswordField.Translation; 
            
            tbDatabase.Text = this.MainVM.AppRepository.DbName; 
            tbSchema.Text = this.MainVM.AppRepository.DbSchema; 
            tbUsername.Text = this.MainVM.AppRepository.DbUsername; 
            pbPassword.Password = this.MainVM.AppRepository.DbPassword; 
        }

        private void InitButtons()
        {
            btnRecover.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.RecoverField.Translation) ? SettingsEntity.RecoverField.English : SettingsEntity.RecoverField.Translation; 
            btnSave.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.SaveField.Translation) ? SettingsEntity.SaveField.English : SettingsEntity.SaveField.Translation; 
            btnCancel.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(SettingsEntity.CancelField.Translation) ? SettingsEntity.CancelField.English : SettingsEntity.CancelField.Translation; 
        }

        public void UpdateAppRepository()
        {
            try 
            {
                this.MainVM.AppRepository.Update(cbLanguage.Text, cbAutoSave.Text, System.Convert.ToInt32(cbFontSize.Text), 
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
            ((MainVM)this.DataContext).SettingsView = null; 
        }
    }
}

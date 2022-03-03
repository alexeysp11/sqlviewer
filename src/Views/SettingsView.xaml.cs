using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        private MainVM MainVM { get; set; }

        public SettingsView()
        {
            InitializeComponent();

            Loaded += (o, e) => 
            {
                this.MainVM = (MainVM)this.DataContext; 
                this.MainVM.SettingsView = this; 
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
            lblPreferencesEditor.Content = "Editor"; 

            lblLanguage.Content = "Language"; 
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

            lblAutoSave.Content = "Auto save"; 
            cbAutoSave.Text = this.MainVM.AppRepository.AutoSave.ToString();
            cbiAutoSaveEnabled.Content = "Enabled"; 
            cbiAutoSaveDisabled.Content = "Disabled"; 

            lblFontSize.Content = "Font size"; 
            cbFontSize.Text = this.MainVM.EnumDecoder.GetFontSizeName(this.MainVM.AppRepository.FontSize); 
            lblFontFamily.Content = "Font family"; 
            cbFontFamily.Text = this.MainVM.AppRepository.FontFamily.ToString(); 
            lblTabSize.Content = "Tab size"; 
            cbTabSize.Text = this.MainVM.EnumDecoder.GetTabSizeName(this.MainVM.AppRepository.TabSize); 

            lblWordWrap.Content = "Word wrap"; 
            cbWordWrap.Text = this.MainVM.AppRepository.WordWrap.ToString(); 
            cbiWordWrapEnabled.Content = "Enabled"; 
            cbiWordWrapDisabled.Content = "Disabled"; 
        }

        private void InitPreferencesDb()
        {
            lblPreferencesDb.Content = "DB"; 
            lblDefaultRdbms.Content = "Default RDBMS"; 
            cbDefaultRdbms.Text = this.MainVM.AppRepository.DefaultRdbms.ToString(); 
            lblActiveRdbms.Content = "Active RDBMS"; 
            cbActiveRdbms.Text = this.MainVM.AppRepository.ActiveRdbms.ToString(); 

            tbDatabase.Text = this.MainVM.AppRepository.DbName; 
            tbSchema.Text = this.MainVM.AppRepository.DbSchema; 
            tbUsername.Text = this.MainVM.AppRepository.DbUsername; 
            pbPassword.Password = this.MainVM.AppRepository.DbPassword; 
        }

        private void InitButtons()
        {
            btnRecover.Content = "Recover"; 
            btnSave.Content = "Save"; 
            btnCancel.Content = "Cancel"; 
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

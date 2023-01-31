using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;
using SqlViewer.Entities.PagesEntities; 
using SqlViewer.Helpers; 
using LanguageEnum = SqlViewer.Enums.Common.Language; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Pages
{
    /// <summary>
    /// Interaction logic for SqlPage.xaml
    /// </summary>
    public partial class SqlPage : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        private MainVM MainVM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private SqlPageEntity SqlPageEntity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SqlPage()
        {
            InitializeComponent();

            Loaded += (o, e) => 
            {
                this.MainVM = (MainVM)this.DataContext; 
                this.MainVM.VisualVM.SqlPage = this; 
                this.SqlPageEntity = this.MainVM.Translator.SqlPageEntity; 
                Init(); 
            }; 
        }

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            tbSqlPageDb.Text = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? (RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(SqlPageEntity.PathField.Translation) ? SqlPageEntity.PathField.English + ": " : SqlPageEntity.PathField.Translation + ": ") : "DB: "; 
            tblDbName.Text = RepoHelper.AppSettingsRepo.DbName; 
            tbActiveRdbms.Text = RepoHelper.AppSettingsRepo.ActiveRdbms.ToString(); 
            btnSqlPageExecute.Content = SettingsHelper.TranslateUiElement(SqlPageEntity.ExecuteField.English, SqlPageEntity.ExecuteField.Translation); 
        }
    }
}

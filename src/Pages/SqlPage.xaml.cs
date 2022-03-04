using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;
using SqlViewer.Entities.PagesEntities; 
using LanguageEnum = SqlViewer.Enums.Common.Language; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Pages
{
    /// <summary>
    /// Interaction logic for SqlPage.xaml
    /// </summary>
    public partial class SqlPage : UserControl
    {
        private MainVM MainVM { get; set; }
        private SqlPageEntity SqlPageEntity { get; set; }

        public SqlPage()
        {
            InitializeComponent();

            Loaded += (o, e) => 
            {
                this.MainVM = (MainVM)this.DataContext; 
                this.MainVM.SqlPage = this; 
                this.SqlPageEntity = this.MainVM.Translator.SqlPageEntity;  
                Init(); 
            }; 
        }

        public void Init()
        {
            tbSqlPagePath.Text = this.MainVM.AppRepository.Language == LanguageEnum.English || string.IsNullOrEmpty(SqlPageEntity.PathField.Translation) ? SqlPageEntity.PathField.English + ": " : SqlPageEntity.PathField.Translation + ": "; 
            tbActiveRdbms.Text = this.MainVM.AppRepository.ActiveRdbms.ToString(); 
            btnSqlPageExecute.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || string.IsNullOrEmpty(SqlPageEntity.ExecuteField.Translation) ? SqlPageEntity.ExecuteField.English : SqlPageEntity.ExecuteField.Translation; 
        }
    }
}

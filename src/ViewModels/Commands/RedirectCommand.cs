using SqlViewer.Views; 
using SqlViewer.ViewModels; 

namespace SqlViewer.Commands
{
    public class RedirectCommand : System.Windows.Input.ICommand
    {
        private MainVM MainVM; 

        public RedirectCommand(MainVM mainVm)
        {
            this.MainVM = mainVm; 
        }

        public event System.EventHandler CanExecuteChanged; 

        public bool CanExecute(object parameter)
        {
            return true; 
        }

        public void Execute(object parameter)
        {
            string parameterString = parameter as string; 
            switch (parameterString)
            {
                case "SqlQuery":
                    this.MainVM.VisualVM.RedirectToSqlQuery(); 
                    break;

                case "Tables":
                    this.MainVM.VisualVM.RedirectToTables(); 
                    break;

                case "Settings":
                    this.MainVM.VisualVM.OpenView("SettingsView"); 
                    break;

                case "Connections":
                    this.MainVM.VisualVM.OpenView("ConnectionsView"); 
                    break;
                
                case "About":
                    this.MainVM.VisualVM.OpenDocsInBrowser("About page", "User guide", $"{SqlViewer.Helpers.SettingsHelper.GetRootFolder()}\\docs\\About.html"); 
                    break;

                case "SqliteDocs":
                    this.MainVM.VisualVM.OpenDocsInBrowser("SQLite documentation", "Common SQL docs", "https://www.sqlite.org/index.html");
                    break;

                case "PosgresDocs":
                    this.MainVM.VisualVM.OpenDocsInBrowser("PosgresSQL documentation", "Common SQL docs", "https://www.postgresql.org/");
                    break;

                case "MySqlDocs":
                    this.MainVM.VisualVM.OpenDocsInBrowser("MySQL documentation", "Common SQL docs", "https://dev.mysql.com/doc/"); 
                    break;

                case "OracleDocs":
                    this.MainVM.VisualVM.OpenDocsInBrowser("Oracle documentation", "Common SQL docs", "https://docs.oracle.com/en/database/oracle/oracle-database/index.html"); 
                    break;

                default:
                    System.Windows.MessageBox.Show($"Incorrect parameter: '{parameterString}' in RedirectCommand", "Error"); 
                    break;
            }
        }
    }
}

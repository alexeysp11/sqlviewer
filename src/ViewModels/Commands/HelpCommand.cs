using SqlViewer.ViewModels; 

namespace SqlViewer.Commands
{
    public class HelpCommand : System.Windows.Input.ICommand
    {
        private MainVM MainVM; 

        public HelpCommand(MainVM mainVm)
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

                default:
                    System.Windows.MessageBox.Show($"Incorrect parameter: {parameterString}", "Error"); 
                    break;
            }
        }
    }
}

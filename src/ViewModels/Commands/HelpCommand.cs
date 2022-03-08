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
            System.String parameterString = parameter as System.String; 
            if (parameterString == "About")
            {
                this.MainVM.OpenDocsInBrowser("About page", 
                                            "User guide", 
                                            $"{this.MainVM.RootFolder}\\docs\\About.html"); 
            }
            else if (parameterString == "SqliteDocs")
            {
                this.MainVM.OpenDocsInBrowser("SQLite documentation", 
                                            "Common SQL docs", 
                                            "https://www.sqlite.org/index.html"); 
            }
            else if (parameterString == "PosgresDocs")
            {
                this.MainVM.OpenDocsInBrowser("PosgresSQL documentation", 
                                            "Common SQL docs", 
                                            "https://www.postgresql.org/"); 
            }
            else if (parameterString == "MySqlDocs")
            {
                this.MainVM.OpenDocsInBrowser("MySQL documentation", 
                                            "Common SQL docs", 
                                            "https://dev.mysql.com/doc/"); 
            }
            else 
            {
                System.Windows.MessageBox.Show($"Incorrect parameter: {parameterString}", "Error"); 
            }
        }
    }
}
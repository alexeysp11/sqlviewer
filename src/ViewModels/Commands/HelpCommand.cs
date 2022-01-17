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
            if (parameterString == "About")
            {
                this.MainVM.ShowUserGuide("About"); 
            }
            else if (parameterString == "SqliteDocs")
            {
                this.MainVM.ShowSqliteDocs(); 
            }
            else if (parameterString == "PosgresDocs")
            {
                this.MainVM.ShowPosgresDocs(); 
            }
            else if (parameterString == "MySqlDocs")
            {
                this.MainVM.ShowMySqlDocs(); 
            }
            else 
            {
                System.Windows.MessageBox.Show($"Incorrect parameter: {parameterString}", "Error"); 
            }
        }
    }
}
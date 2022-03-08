using SqlViewer.ViewModels; 

namespace SqlViewer.Commands
{
    public class DbCommand : System.Windows.Input.ICommand
    {
        private MainVM MainVM; 

        public DbCommand(MainVM mainVm)
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
            if (parameterString == "SendSql")
            {
                this.MainVM.SendSqlRequest(); 
            }
            else if (parameterString == "New")
            {
                // If SQLite 
                this.MainVM.CreateLocalDb(); 
            }
            else if (parameterString == "Open")
            {
                // If SQLite 
                this.MainVM.OpenLocalDb(); 
            }
            else
            {
                System.Windows.MessageBox.Show($"Incorrect CommandParameter: {parameterString} inside DbCommand", "Exception"); 
            }
        }
    }
}
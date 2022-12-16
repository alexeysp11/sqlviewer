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
            string parameterString = parameter as string; 
            switch (parameterString)
            {
                case "SendSql":
                    this.MainVM.DbVM.SendSqlRequest(); 
                    break;
                    
                case "New":
                    // if SQLite
                    this.MainVM.DbVM.CreateLocalDb(); 
                    break;
                    
                case "Open":
                    // if SQLite
                    this.MainVM.DbVM.OpenLocalDb(); 
                    break;

                default: 
                    System.Windows.MessageBox.Show($"Incorrect CommandParameter: '{parameterString}' inside DbCommand", "Exception"); 
                    break; 
            }
        }
    }
}
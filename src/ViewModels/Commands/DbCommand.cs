using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

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
                    this.MainVM.DataVM.MainDbClient.SendSqlRequest(); 
                    break;
                    
                case "New":
                    this.MainVM.DataVM.MainDbClient.CreateDb(); 
                    break;
                    
                case "Open":
                    this.MainVM.DataVM.MainDbClient.OpenDb(); 
                    break;

                default: 
                    System.Windows.MessageBox.Show($"Incorrect CommandParameter: '{parameterString}' inside DbCommand", "Exception"); 
                    break; 
            }
        }
    }
}

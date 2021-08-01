using SqlViewer.ViewModels; 

namespace SqlViewer.Commands
{
    public class SendSqlCommand : System.Windows.Input.ICommand 
    {
        private MainVM MainVM; 

        public SendSqlCommand(MainVM mainVm)
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
            this.MainVM.SendSqlRequest(); 
        }
    }
}
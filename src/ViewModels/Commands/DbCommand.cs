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
            if (parameterString == "New")
            {
                this.MainVM.CreateNewDb(); 
            }
            else if (parameterString == "Open")
            {
                this.MainVM.OpenExistingDb(); 
            }
            else
            {
                System.Windows.MessageBox.Show($"Incorrect CommandParameter: {parameterString} inside DbCommand", "Exception"); 
            }
        }
    }
}
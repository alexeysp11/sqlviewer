using SqlViewer.ViewModels; 

namespace SqlViewer.Commands
{
    public class AppCommand : System.Windows.Input.ICommand
    {
        private MainVM MainVM; 

        public AppCommand(MainVM mainVm)
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
            if (parameterString == "ExitApplication")
            {
                this.MainVM.ExitApplication(); 
            }
            else if (parameterString == "RecoverSettings")
            {
                this.MainVM.RecoverSettings(); 
            }
            else if (parameterString == "SaveSettings")
            {
                this.MainVM.SaveSettings(); 
            }
            else if (parameterString == "CancelSettings")
            {
                this.MainVM.CancelSettings(); 
            }
            else 
            {
                System.Windows.MessageBox.Show($"Incorrect CommandParameter: {parameterString} inside AppCommand", "Exception"); 
            }
        }
    }
}
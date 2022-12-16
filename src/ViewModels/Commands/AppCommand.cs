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
            switch (parameterString)
            {
                case "ExitApplication":
                    this.MainVM.ExitApplication(); 
                    break;
                    
                case "RecoverSettings":
                    this.MainVM.RecoverSettings(); 
                    break;
                    
                case "SaveSettings":
                    this.MainVM.SaveSettings(); 
                    break;
                    
                case "CancelSettings":
                    this.MainVM.CancelSettings(); 
                    break;
                    
                default: 
                    System.Windows.MessageBox.Show($"Incorrect CommandParameter: '{parameterString}' inside AppCommand", "Exception"); 
                    break; 
            }
        }
    }
}
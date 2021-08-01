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
            this.MainVM.ShowHelpWindow(); 
        }
    }
}
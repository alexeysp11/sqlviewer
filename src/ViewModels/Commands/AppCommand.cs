using SqlViewer.ViewModels; 

namespace SqlViewer.Commands
{
    public class AppCommand : System.Windows.Input.ICommand
    {
        private MainVM MainVM; 

        public AppCommand(MainVM mainVm) => this.MainVM = mainVm; 

#pragma warning disable 67
        public event System.EventHandler CanExecuteChanged; 
#pragma warning restore 67

        public bool CanExecute(object parameter) => true; 

        public void Execute(object parameter) => this.MainVM.PreprocCommandParameter((string)(parameter as string)); 
    }
}
using SqlViewer.Views; 
using SqlViewer.ViewModels; 

namespace SqlViewer.Commands
{
    public class RedirectCommand : System.Windows.Input.ICommand
    {
        private MainVM MainVM; 

        public RedirectCommand(MainVM mainVm)
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
            if (parameterString == "SqlQuery")
            {
                this.MainVM.RedirectToSqlQuery(); 
            }
            else if (parameterString == "Tables")
            {
                this.MainVM.RedirectToTables(); 
            }
            else if (parameterString == "Settings")
            {
                this.MainVM.OpenView("SettingsView"); 
            }
            else if (parameterString == "Options")
            {
                this.MainVM.OpenView("OptionsView"); 
            }
            else if (parameterString == "Connections")
            {
                this.MainVM.OpenView("ConnectionsView"); 
            }
            else
            {
                System.Windows.MessageBox.Show($"Incorrect CommandParameter: {parameterString} inside RedirectCommand", "Exception"); 
            }
        }
    }
}
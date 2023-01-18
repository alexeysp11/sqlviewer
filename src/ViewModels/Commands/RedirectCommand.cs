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
            switch (parameterString)
            {
                case "SqlQuery":
                    this.MainVM.VisualVM.RedirectToSqlQuery(); 
                    break;

                case "Tables":
                    this.MainVM.VisualVM.RedirectToTables(); 
                    break;

                case "Settings":
                    this.MainVM.VisualVM.OpenView("SettingsView"); 
                    break;

                case "Options":
                    this.MainVM.VisualVM.OpenView("OptionsView"); 
                    break;

                case "Connections":
                    this.MainVM.VisualVM.OpenView("ConnectionsView"); 
                    break;

                case "Network":
                    this.MainVM.VisualVM.OpenView("NetworkView"); 
                    break;

                case "CustomFiles":
                    this.MainVM.VisualVM.OpenView("CustomFilesView"); 
                    break;

                default:
                    System.Windows.MessageBox.Show($"Incorrect parameter: '{parameterString}' in RedirectCommand", "Error"); 
                    break;
            }
        }
    }
}

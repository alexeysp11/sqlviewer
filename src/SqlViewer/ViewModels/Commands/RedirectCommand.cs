namespace SqlViewer.ViewModels.Commands;

public class RedirectCommand(MainVM mainVm) : System.Windows.Input.ICommand
{
    private readonly MainVM MainVM = mainVm;

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
                MainVM.VisualVM.RedirectToSqlQuery();
                break;

            case "Tables":
                MainVM.VisualVM.RedirectToTables();
                break;

            case "Settings":
                MainVM.VisualVM.OpenView("SettingsView");
                break;

            case "Options":
                MainVM.VisualVM.OpenView("OptionsView");
                break;

            case "Connections":
                MainVM.VisualVM.OpenView("ConnectionsView");
                break;

            default:
                System.Windows.MessageBox.Show($"Incorrect parameter: '{parameterString}' in RedirectCommand", "Error");
                break;
        }
    }
}

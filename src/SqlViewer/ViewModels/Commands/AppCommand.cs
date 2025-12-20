namespace SqlViewer.ViewModels.Commands;

public class AppCommand(MainVM mainVm) : System.Windows.Input.ICommand
{
    private readonly MainVM MainVM = mainVm;

    public event EventHandler CanExecuteChanged;

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
                MainVM.ExitApplication();
                break;

            case "RecoverSettings":
                MainVM.RecoverSettings();
                break;

            case "SaveSettings":
                MainVM.SaveSettings();
                break;

            case "CancelSettings":
                MainVM.CancelSettings();
                break;

            default:
                System.Windows.MessageBox.Show($"Incorrect CommandParameter: '{parameterString}' inside AppCommand", "Exception");
                break;
        }
    }
}
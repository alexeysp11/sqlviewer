using System.Windows;

namespace SqlViewer.ViewModels.Commands;

public class AppCommand(MainVM mainVm) : System.Windows.Input.ICommand
{
    private readonly MainVM MainVM = mainVm;

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
        try
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
                    throw new Exception($"Incorrect CommandParameter: '{parameterString}' inside AppCommand");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
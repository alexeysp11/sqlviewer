using System.Windows;

namespace SqlViewer.ViewModels.Commands;

public class RedirectCommand(MainVM mainVm) : System.Windows.Input.ICommand
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
                    throw new Exception($"Incorrect parameter: '{parameterString}' in RedirectCommand");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

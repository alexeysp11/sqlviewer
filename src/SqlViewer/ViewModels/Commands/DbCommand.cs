using System.Windows;

namespace SqlViewer.ViewModels.Commands;

public class DbCommand(MainVM mainVm) : System.Windows.Input.ICommand
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
                case "SendSql":
                    MainVM.DataVM.SendSqlRequest();
                    break;

                case "New":
                    MainVM.DataVM.CreateDb();
                    break;

                case "Open":
                    MainVM.DataVM.OpenDb();
                    break;

                default:
                    throw new Exception($"Incorrect CommandParameter: '{parameterString}' inside DbCommand");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

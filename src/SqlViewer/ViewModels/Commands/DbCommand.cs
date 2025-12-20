namespace SqlViewer.ViewModels.Commands;

public class DbCommand(MainVM mainVm) : System.Windows.Input.ICommand
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
                System.Windows.MessageBox.Show($"Incorrect CommandParameter: '{parameterString}' inside DbCommand", "Exception");
                break;
        }
    }
}

using System.Windows;

namespace SqlViewer.ViewModels.Commands;

public class HelpCommand : System.Windows.Input.ICommand
{
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
        try
        {
            string parameterString = parameter as string;
            switch (parameterString)
            {
                case "About":
                    VisualVM.OpenDocsInBrowser("About page", "User guide", $"{Helpers.SettingsHelper.RootFolder}\\docs\\About.html");
                    break;

                case "SqliteDocs":
                    VisualVM.OpenDocsInBrowser("SQLite documentation", "Common SQL docs", "https://www.sqlite.org/index.html");
                    break;

                case "PosgresDocs":
                    VisualVM.OpenDocsInBrowser("PosgresSQL documentation", "Common SQL docs", "https://www.postgresql.org/");
                    break;

                case "MySqlDocs":
                    VisualVM.OpenDocsInBrowser("MySQL documentation", "Common SQL docs", "https://dev.mysql.com/doc/");
                    break;

                default:
                    throw new Exception($"Incorrect parameter: {parameterString}");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

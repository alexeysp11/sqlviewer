using System.Windows;
using System.Windows.Controls;
using SqlViewer.Views;
using SqlViewer.Pages;
using UserControlsMenu = SqlViewer.UserControls.Menu;

namespace SqlViewer.ViewModels;

public class VisualVM(MainVM mainVM)
{
    private MainVM MainVM { get; set; } = mainVM;

    public Window LoginView { get; set; }
    public Window SettingsView { get; set; }
    public Window ConnectionsView { get; set; }

    public UserControl Menu { get; set; }
    public UserControl SqlPage { get; set; }
    public UserControl TablesPage { get; set; }

    #region Common UI methods 
    public void InitUI()
    {
        ((SettingsView)SettingsView).Init();

        ((UserControlsMenu)Menu).Init();

        ((SqlPage)SqlPage).Init();
        ((TablesPage)TablesPage).Init();
    }
    #endregion  // Common UI methods 

    #region Views methods
    public void OpenView(string viewName)
    {
        try
        {
            Type type = System.Type.GetType("SqlViewer.Views." + viewName);
            Window win = System.Activator.CreateInstance(type) as Window;
            win.DataContext = MainVM;
            win.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public static void OpenDocsInBrowser(string docName, string title, string filePath)
    {
        string msg = "Do you want to open " + docName + " in your browser?";
        if (MessageBox.Show(msg, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            System.Diagnostics.Process process = new();
            try
            {
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = filePath;
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    #endregion  // Views methods

    #region Pages methods
    public void RedirectToSqlQuery()
    {
        HideAllPages();
        DisableAllPages();

        MainVM.MainWindow.SqlPage.Visibility = Visibility.Visible;
        MainVM.MainWindow.SqlPage.IsEnabled = true;
    }

    public void RedirectToTables()
    {
        MainVM.DataVM.DisplayTablesInDb();

        HideAllPages();
        DisableAllPages();

        MainVM.MainWindow.TablesPage.Visibility = Visibility.Visible;
        MainVM.MainWindow.TablesPage.IsEnabled = true;
    }

    private void HideAllPages()
    {
        MainVM.MainWindow.SqlPage.Visibility = Visibility.Collapsed;
        MainVM.MainWindow.TablesPage.Visibility = Visibility.Collapsed;
    }

    private void DisableAllPages()
    {
        MainVM.MainWindow.TablesPage.IsEnabled = false;
        MainVM.MainWindow.TablesPage.IsEnabled = false;
    }
    #endregion  // Pages methods
}

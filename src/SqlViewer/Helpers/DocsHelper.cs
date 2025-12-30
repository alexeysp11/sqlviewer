using System.Diagnostics;
using System.IO;
using System.Windows;

namespace SqlViewer.Helpers;

public static class DocsHelper
{
    public static void OpenDocsInBrowser(string title, string filePath)
    {
        string msg = "Do you want to open " + title + " in your browser?";
        if (MessageBox.Show(msg, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            Process process = new();
            try
            {
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = filePath;
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
    }

    public static string SaveContentAsFile(string content, string fileName)
    {
        File.WriteAllText(fileName, content);
        return fileName;
    }
}

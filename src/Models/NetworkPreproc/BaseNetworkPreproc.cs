using System.Windows; 
using SqlViewer.Models.AppBranches; 

namespace SqlViewer.Models.NetworkPreproc
{
    public abstract class BaseNetworkPreproc
    {
        protected void RunNetworkExtension()
        {
            SqlViewer.Helpers.FileSysHelper.ExecuteCmd("cd " + SqlViewer.Helpers.SettingsHelper.GetRootFolder() + " && runnetwork.cmd"); 
        }

        protected void InitNetworkExtension(INetworkPreproc networkPreproc)
        {
            // Write data into settings file for the network extension 
            string filename = System.IO.Path.Combine(SqlViewer.Helpers.SettingsHelper.GetRootFolder(), "data/SqlViewerNetwork/settings.txt"); 
            SqlViewer.Helpers.FileSysHelper.WriteIntoFile(filename, networkPreproc.GetType().ToString());

            string msg = "Do you want to run an instance of network extension?"; 
            if (System.Windows.MessageBox.Show(msg, "Running network extension", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                RunNetworkExtension(); 
            }
        }
    }
}

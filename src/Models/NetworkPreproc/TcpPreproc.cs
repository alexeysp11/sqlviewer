namespace SqlViewer.Models.NetworkPreproc
{
    public class TcpPreproc : INetworkPreproc
    {
        public TcpPreproc()
        {
            StartServer(); 
        }

        private void StartServer()
        {
            //(this.Server = new HttpNetworkServer()).StartServer(); 
            SqlViewer.Helpers.FileSysHelper.ExecuteCmd("cd " + SqlViewer.Helpers.SettingsHelper.GetRootFolder() + " && runnetwork.cmd"); 
            //System.Windows.MessageBox.Show("StartServer"); 
        }
    }
}

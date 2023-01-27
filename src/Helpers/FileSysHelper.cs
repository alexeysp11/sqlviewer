using Microsoft.Win32;

namespace SqlViewer.Helpers
{
    public static class FileSysHelper
    {
        public static string OpenLocalFile()
        {
            string path = string.Empty; 
            try
            {
                OpenFileDialog ofd = new OpenFileDialog(); 
                ofd.Filter = SettingsHelper.GetFilterFileSystemString();
                if (ofd.ShowDialog() == true) {}

                path = ofd.FileName; 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
            return path; 
        }
        public static void SaveLocalFile()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = SettingsHelper.GetFilterFileSystemString(); 
                if (sfd.ShowDialog() == true)
                    System.IO.File.WriteAllText(sfd.FileName, string.Empty);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        public static void WriteIntoFile(string filename, string content)
        {
            using (var outputFile = new System.IO.StreamWriter(filename))
            {
                outputFile.Write(content);
            }
        }
        public static string ReadFromFile()
        {
            return ""; 
        }

        public static void ExecuteCmd(string cmdCommand)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + cmdCommand;
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}

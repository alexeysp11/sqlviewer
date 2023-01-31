using Microsoft.Win32;

namespace SqlViewer.Helpers
{
    /// <summary>
    /// Helper for file system operations 
    /// </summary>
    public static class FileSysHelper
    {
        /// <summary>
        /// Opens a local file 
        /// </summary>
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
        /// <summary>
        /// Saves a local file 
        /// </summary>
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

        /// <summary>
        /// Writes a string into a local file 
        /// </summary>
        public static void WriteIntoFile(string filename, string content)
        {
            using (var outputFile = new System.IO.StreamWriter(filename))
            {
                outputFile.Write(content);
            }
        }
        /// <summary>
        /// Reads text from a local file 
        /// </summary>
        public static string ReadFromFile()
        {
            return ""; 
        }

        /// <summary>
        /// Executes a command using command line 
        /// </summary>
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

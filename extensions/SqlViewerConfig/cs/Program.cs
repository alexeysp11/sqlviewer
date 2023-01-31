namespace SqlViewerConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            Configurator configurator = new Configurator(); 

            System.Console.WriteLine("Configure settings..."); 
            configurator.GetConfigSettings(); 
            System.Console.WriteLine("Settings are configured."); 

            if (configurator.GetSelectedLanguage() == "cpp")
            {
                configurator.InvokeCppConfig(); 
                System.Console.ReadKey();
            }
            else
            {
                System.Console.WriteLine("Create folders..."); 
                configurator.CreateFolders(); 
                System.Console.WriteLine("Folders are created."); 
                
                // System.Console.WriteLine("Start making backups..."); 
                // configurator.BackupData(); 
                // System.Console.WriteLine("Backups are made."); 

                System.Console.WriteLine("Create local files..."); 
                configurator.CreateLocalFiles(); 
                System.Console.WriteLine("Local files are created."); 
            }
            System.Console.WriteLine("Initialize databases..."); 
            configurator.InitDatabases(); 
            System.Console.WriteLine("Databases are initialized."); 
            
            System.Console.WriteLine("Configuration is finished successfully!"); 
        }
    }
}

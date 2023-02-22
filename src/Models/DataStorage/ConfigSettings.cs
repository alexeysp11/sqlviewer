namespace SqlViewer.Models.DataStorage
{
    /// <summary>
    /// Stores settings from config file 
    /// </summary>
    public sealed class ConfigSettings
    {
        /// <summary>
        /// Gets if the application can start in case of data folder is not configured 
        /// <summary>
        public bool StartWithoutDataConfiguration { get; set; }
        /// <summary>
        /// Gets if the application can run using defalt data, in case of data folder is not configured 
        /// <summary>
        public bool RunWithoutDataConfiguration { get; set; }
        /// <summary>
        /// 
        /// <summary>
        public LoggingSettings LoggingSettings { get; set; }
    }
}

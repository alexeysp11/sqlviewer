namespace SqlViewer.Models.DataStorage 
{
    /// <summary>
    /// Class for storing logging related settings
    /// <summary>
    public sealed class LoggingSettings
    {
        /// <summary>
        /// 
        /// <summary>
        public string[] Database { get; set; } = null!;
        /// <summary>
        /// 
        /// <summary>
        public string[] DotNet { get; set; } = null!;
    }
}
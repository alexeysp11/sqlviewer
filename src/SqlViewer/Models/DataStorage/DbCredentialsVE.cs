using System.Windows.Controls; 

namespace SqlViewer.Models.DataStorage
{
    /// <summary>
    /// Stores objects of visual elements, representing database credentials on SettingsView or LoginView, 
    /// that are going to be initialized  
    /// </summary>
    public sealed class DbCredentialsVE
    {
        /// <summary>
        /// ComboBox for choosing Active RDBMS 
        /// <summary>
        public ComboBox cbActiveRdbms { get; set; }
        /// <summary>
        /// TextBox for specifying Server 
        /// <summary>
        public TextBox tbServer { get; set; }
        /// <summary>
        /// TextBox for specifying Database 
        /// <summary>
        public TextBox tbDatabase { get; set; }
        /// <summary>
        /// TextBox for specifying Port 
        /// <summary>
        public TextBox tbPort { get; set; }
        /// <summary>
        /// TextBox for specifying Schema 
        /// <summary>
        public TextBox tbSchema { get; set; }
        /// <summary>
        /// TextBox for specifying Username 
        /// <summary>
        public TextBox tbUsername { get; set; }
        /// <summary>
        /// PasswordBox for specifying Password 
        /// <summary>
        public PasswordBox pbPassword { get; set; }
        /// <summary>
        /// Button for selecting a file for local database 
        /// <summary>
        public Button btnDatabase { get; set; }
    }
}

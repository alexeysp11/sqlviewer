using SqlViewer.Helpers; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.DataStorage
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseSettings
    {
        #region Properties
        /// <summary>
        /// Default RDBMS 
        /// </summary>
        public RdbmsEnum DefaultRdbms { get; private set; }
        /// <summary>
        /// Active RDBMS 
        /// </summary>
        public RdbmsEnum ActiveRdbms { get; private set; }

        /// <summary>
        /// Parameter Host for database connection 
        /// </summary>
        public string DbHost { get; private set; }
        /// <summary>
        /// Parameter Port for database connection 
        /// </summary>
        public string DbPort { get; private set; }
        /// <summary>
        /// Parameter Name for database connection 
        /// </summary>
        public string DbName { get; private set; }
        /// <summary>
        /// Parameter Schema for database connection 
        /// </summary>
        public string DbSchema { get; private set; }
        /// <summary>
        /// Parameter Username for database connection 
        /// </summary>
        public string DbUsername { get; private set; }
        /// <summary>
        /// Parameter Password for database connection 
        /// </summary>
        public string DbPassword { get; private set; }
        #endregion  // Properties
        
        #region Public methods 
        /// <summary>
        /// Sets Default RDBMS 
        /// </summary>
        public void SetDefaultRdbms(string defaultRdbms) 
        {
            if (string.IsNullOrEmpty(defaultRdbms)) throw new System.Exception("Parameter 'defaultRdbms' could not be null or empty"); 
            DefaultRdbms = RepoHelper.EnumEncoder.GetRdbmsEnum(defaultRdbms); 
        }
        /// <summary>
        /// Sets Active RDBMS 
        /// </summary>
        public void SetActiveRdbms(string activeRdbms) 
        {
            if (string.IsNullOrEmpty(activeRdbms)) throw new System.Exception("Parameter 'activeRdbms' could not be null or empty"); 
            ActiveRdbms = RepoHelper.EnumEncoder.GetRdbmsEnum(activeRdbms); 
        }

        /// <summary>
        /// Sets parameter Host for database connection 
        /// </summary>
        public void SetDbHost(string dbHost) 
        {
            DbHost = dbHost; 
        }
        /// <summary>
        /// Sets parameter Name for database connection 
        /// </summary>
        public void SetDbName(string dbName) 
        {
            DbName = dbName; 
        }
        /// <summary>
        /// Sets parameter Port for database connection 
        /// </summary>
        public void SetDbPort(string dbPort) 
        {
            DbPort = dbPort; 
        }
        /// <summary>
        /// Sets parameter Schema for database connection 
        /// </summary>
        public void SetDbSchema(string dbSchema) 
        {
            DbSchema = dbSchema; 
        }
        /// <summary>
        /// Sets parameter Username for database connection 
        /// </summary>
        public void SetDbUsername(string dbUsername) 
        {
            DbUsername = dbUsername; 
        }
        /// <summary>
        /// Sets parameter Password for database connection 
        /// </summary>
        public void SetDbPassword(string dbPassword) 
        {
            DbPassword = dbPassword; 
        }
        #endregion  // Public methods 
    }
}

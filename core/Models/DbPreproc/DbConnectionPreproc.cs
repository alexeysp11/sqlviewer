using SqlViewer.Helpers; 
using SqlViewerDatabase.DbConnections; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.DbPreproc
{
    /// <summary>
    /// Performs database connection related operations 
    /// </summary>
    public class DbConnectionPreproc 
    {
        /// <summary>
        /// Database connection instance on the App layer  
        /// </summary>
        public ICommonDbConnectionSV AppDbConnection { get; private set; } = new SqliteDbConnection($"{SettingsHelper.GetRootFolder()}\\data\\app.db");
        /// <summary>
        /// Database connection instance on the User layer  
        /// </summary>
        public ICommonDbConnectionSV UserDbConnection { get; set; }

        /// <summary>
        /// initializes UserRdbmsPreproc
        /// </summary>
        public void InitUserDbConnection()
        {
            try
            {
                var appSettingsRepo = RepoHelper.AppSettingsRepo; 
                if (appSettingsRepo == null)
                    throw new System.Exception("RepoHelper.AppSettingsRepo is not assigned."); 
                switch (appSettingsRepo.DatabaseSettings.ActiveRdbms)
                {
                    case RdbmsEnum.SQLite: 
                        if (!string.IsNullOrEmpty(appSettingsRepo.DatabaseSettings.DbName))
                            UserDbConnection = new SqliteDbConnection(appSettingsRepo.DatabaseSettings.DbName);
                        break; 

                    case RdbmsEnum.PostgreSQL: 
                        UserDbConnection = new PgDbConnection();
                        break;

                    case RdbmsEnum.MySQL: 
                        UserDbConnection = new MysqlDbConnection();
                        break;

                    case RdbmsEnum.Oracle: 
                        UserDbConnection = new OracleDbConnection();
                        break;

                    default:
                        throw new System.Exception($"Unable to call RDBMS preprocessing unit, incorrect ActiveRdbms: {appSettingsRepo.DatabaseSettings.ActiveRdbms}.");
                }
            }
            catch (System.Exception ex)
            {
                throw ex;  
            }
        }

        /// <summary>
        /// Initializes DbConnection after OpenFileDialog (could only be used when SQLite is selected as an active RDBMS)
        /// </summary>
        public void InitLocalDbConnection(string path)
        {
            try
            {
                var databaseSettings = RepoHelper.AppSettingsRepo.DatabaseSettings; 
                if (databaseSettings.ActiveRdbms != RdbmsEnum.SQLite)
                    throw new System.Exception("In order to initialize local database connection SQLite should be selected as an active RDBMS"); 

                UserDbConnection = new SqliteDbConnection(path);
                databaseSettings.SetDbName(path); 
            }
            catch (System.Exception ex)
            {
                throw ex;  
            }
        }
    }
}

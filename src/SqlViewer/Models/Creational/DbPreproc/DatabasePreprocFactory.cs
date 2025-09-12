using SqlViewer.Helpers; 
using SqlViewer.Models.DbPreproc.DbDatabasePreproc; 
using SqlViewer.ViewModels; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.Creational.DbPreproc
{
    /// <summary>
    /// Creates database related preproc classes 
    /// </summary>
    public class DatabasePreprocFactory
    {
        /// <summary>
        /// Instance of database related preproc class for SQLite
        /// </summary>
        public IDbDatabasePreproc SqliteDatabasePreproc { get; private set; } 
        /// <summary>
        /// Instance of database related preproc class for PostgreSQL
        /// </summary>
        public IDbDatabasePreproc PgDatabasePreproc { get; private set; } 
        /// <summary>
        /// Instance of database related preproc class for MySQL
        /// </summary>
        public IDbDatabasePreproc MysqlDatabasePreproc { get; private set; } 
        /// <summary>
        /// Instance of database related preproc class for Oracle
        /// </summary>
        public IDbDatabasePreproc OracleDatabasePreproc { get; private set; } 

        /// <summary>
        /// Constructor of DatabasePreprocFactory
        /// </summary>
        public DatabasePreprocFactory(MainVM mainVM)
        {
            SqliteDatabasePreproc = new SqliteDatabasePreproc(mainVM);
            PgDatabasePreproc = new PgDatabasePreproc(mainVM);
            MysqlDatabasePreproc = new MysqlDatabasePreproc(mainVM);
            OracleDatabasePreproc = new OracleDatabasePreproc(mainVM);
        }

        /// <summary>
        /// Gets instance of database related preproc class
        /// </summary>
        public IDbDatabasePreproc GetDbDatabasePreproc()
        {
            switch (RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms)
            {
                case RdbmsEnum.SQLite: 
                    return SqliteDatabasePreproc; 
                
                case RdbmsEnum.PostgreSQL: 
                    return PgDatabasePreproc; 

                case RdbmsEnum.MySQL: 
                    return MysqlDatabasePreproc; 

                case RdbmsEnum.Oracle: 
                    return OracleDatabasePreproc; 

                default:
                    throw new System.Exception($"Unable to call RDBMS preprocessing unit, incorrect ActiveRdbms: {RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms}.");
            }
        }
    }
}

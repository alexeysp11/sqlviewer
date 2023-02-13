using SqlViewer.Helpers; 
using SqlViewer.Models.DbPreproc.DbDatabasePreproc; 
using SqlViewer.ViewModels; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.Creational.DbPreproc
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabasePreprocFactory
    {
        /// <summary>
        /// 
        /// </summary>
        public IDbDatabasePreproc SqliteDatabasePreproc { get; private set; } 
        /// <summary>
        /// 
        /// </summary>
        public IDbDatabasePreproc PgDatabasePreproc { get; private set; } 
        /// <summary>
        /// 
        /// </summary>
        public IDbDatabasePreproc MysqlDatabasePreproc { get; private set; } 
        /// <summary>
        /// 
        /// </summary>
        public IDbDatabasePreproc OracleDatabasePreproc { get; private set; } 

        /// <summary>
        /// 
        /// </summary>
        public DatabasePreprocFactory(MainVM mainVM)
        {
            SqliteDatabasePreproc = new SqliteDatabasePreproc(mainVM);
            PgDatabasePreproc = new PgDatabasePreproc(mainVM);
            MysqlDatabasePreproc = new MysqlDatabasePreproc(mainVM);
            OracleDatabasePreproc = new OracleDatabasePreproc(mainVM);
        }

        /// <summary>
        /// 
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

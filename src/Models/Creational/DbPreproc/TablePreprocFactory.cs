using SqlViewer.Helpers; 
using SqlViewer.Models.DbPreproc.DbTablePreproc; 
using SqlViewer.ViewModels; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.Creational.DbPreproc
{
    /// <summary>
    /// Creates table related preproc classes 
    /// </summary>
    public class TablePreprocFactory
    {
        /// <summary>
        /// Instance of table related preproc class for SQLite
        /// </summary>
        public IDbTablePreproc SqliteTablePreproc { get; private set; } 
        /// <summary>
        /// Instance of table related preproc class for PostgreSQL
        /// </summary>
        public IDbTablePreproc PgTablePreproc { get; private set; } 
        /// <summary>
        /// Instance of table related preproc class for MySQL
        /// </summary>
        public IDbTablePreproc MysqlTablePreproc { get; private set; } 
        /// <summary>
        /// Instance of table related preproc class for Oracle
        /// </summary>
        public IDbTablePreproc OracleTablePreproc { get; private set; } 

        /// <summary>
        /// Constructor of TablePreprocFactory
        /// </summary>
        public TablePreprocFactory(MainVM mainVM)
        {
            SqliteTablePreproc = new SqliteTablePreproc(mainVM);
            PgTablePreproc = new PgTablePreproc(mainVM);
            MysqlTablePreproc = new MysqlTablePreproc(mainVM);
            OracleTablePreproc = new OracleTablePreproc(mainVM);
        }

        /// <summary>
        /// Gets instance of table related preproc class
        /// </summary>
        public IDbTablePreproc GetDbTablePreproc()
        {
            switch (RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms)
            {
                case RdbmsEnum.SQLite: 
                    return SqliteTablePreproc; 
                
                case RdbmsEnum.PostgreSQL: 
                    return PgTablePreproc; 

                case RdbmsEnum.MySQL: 
                    return MysqlTablePreproc; 

                case RdbmsEnum.Oracle: 
                    return OracleTablePreproc; 

                default:
                    throw new System.Exception($"Unable to call RDBMS preprocessing unit, incorrect ActiveRdbms: {RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms}.");
            }
        }
    }
}

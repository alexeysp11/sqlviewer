using System.Data; 
using System.Linq;
using SqlViewerDatabase.DbConnections; 

namespace SqlViewer.Utils.Language
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseTranslator
    {
        /// <summary>
        /// 
        /// </summary>
        private SqliteDbConnection AppDbConnection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public void SetAppDbConnection(SqliteDbConnection appDbConnection)
        {
            this.AppDbConnection = appDbConnection; 
        }

        /// <summary>
        /// 
        /// </summary>
        public DataTable Translate(string sql)
        {
            try
            {
                return this.AppDbConnection.ExecuteSqlCommand(sql); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TranslateSingleWord(DataTable dt, string rowName, string wordEnglish)
        {
            string result = string.Empty; 
            try
            {
                var row = dt
                    .AsEnumerable()
                    .Where(row => row.Field<string>("english") == wordEnglish)
                    .First(); 
                result = row[rowName].ToString(); 
            }
            catch (System.Exception ex)
            {

            }
            return result; 
        }
    }
}
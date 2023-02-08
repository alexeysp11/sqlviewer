using System.Data; 
using System.Linq;
using SqlViewerDatabase.DbConnections; 

namespace SqlViewer.Utils.Language
{
    /// <summary>
    /// Base class of Translator
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
            catch (System.Exception)
            {
                throw; 
            }
        }

        /// <summary>
        /// Finds translation of an english word
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
            catch (System.Exception) {}
            return result; 
        }
    }
}
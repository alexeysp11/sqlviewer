using System.Data; 
using System.Linq;
using SqlViewerDatabase.DbConnections; 

namespace SqlViewer.Utils.Language
{
    public abstract class BaseTranslator
    {
        private SqliteDbConnection AppDbConnection { get; set; }

        public void SetAppDbConnection(SqliteDbConnection appDbConnection)
        {
            this.AppDbConnection = appDbConnection; 
        }

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
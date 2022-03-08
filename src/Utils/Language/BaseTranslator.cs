using System.Data; 
using System.Linq;
using SqlViewer.Models.DbConnections; 

namespace SqlViewer.Utils.Language
{
    public abstract class BaseTranslator
    {
        private SqliteDbConnection AppDbConnection { get; set; }

        public void SetAppDbConnection(SqliteDbConnection appDbConnection)
        {
            this.AppDbConnection = appDbConnection; 
        }

        public DataTable Translate(System.String sql)
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

        public System.String TranslateSingleWord(DataTable dt, System.String rowName, System.String wordEnglish)
        {
            System.String result = System.String.Empty; 
            try
            {
                var row = dt
                    .AsEnumerable()
                    .Where(row => row.Field<System.String>("english") == wordEnglish)
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
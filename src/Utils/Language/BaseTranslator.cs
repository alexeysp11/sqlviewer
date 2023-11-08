using System.Data; 
using System.Linq;
using Cims.WorkflowLib.DbConnections; 

namespace SqlViewer.Utils.Language
{
    /// <summary>
    /// Base class of Translator
    /// </summary>
    public abstract class BaseTranslator
    {
        /// <summary>
        /// Database connection on the App layer 
        /// </summary>
        private SqliteDbConnection AppDbConnection { get; set; }

        /// <summary>
        /// Sets database connection on the App layer 
        /// </summary>
        public void SetAppDbConnection(SqliteDbConnection appDbConnection) => this.AppDbConnection = appDbConnection; 

        /// <summary>
        /// Translates using input SQL script 
        /// </summary>
        public DataTable Translate(string sql)
        {
            try
            {
                // Check if sql is null or empty 
                return this.AppDbConnection.ExecuteSqlCommand(sql).DataTableResult; 
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
            // Check if dt is null or empty 
            // Check if rowName and wordEnglish are not null or empty 
            string result = string.Empty; 
            try
            {
                var row = dt
                    .AsEnumerable()
                    .Where(row => row.Field<string>("english") == wordEnglish)
                    .FirstOrDefault(); 
                result = row[rowName].ToString(); 
            }
            catch (System.Exception ex) 
            {
                System.Windows.MessageBox.Show(ex.ToString()); 
                string dtStr = string.Empty; 
                foreach(DataRow row in dt.Rows)
                {
                    dtStr += $"rowName: '{rowName}', wordEnglish: '{wordEnglish}', row[rowName]: {row[rowName]}\n"; 
                }
                System.Windows.MessageBox.Show(dtStr, "Content of DataTable"); 
            }
            return result; 
        }
    }
}
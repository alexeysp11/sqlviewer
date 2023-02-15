using System.Collections.Generic; 
using System.Windows; 
using SqlViewer.Helpers; 
using SqlViewer.Models.Logging; 
using SqlViewerDatabase.DbConnections; 

namespace SqlViewer.Models.Creational.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class LoggingWriterCreator : ILoggingWriterCreator
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ILoggingWriter> GetLoggingWriters()
        {
            List<ILoggingWriter> loggingWriters = new List<ILoggingWriter>(); 
            try
            {
                InitDbLogWriters(ref loggingWriters); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return loggingWriters; 
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitDbLogWriters(ref List<ILoggingWriter> loggingWriters)
        {
            try
            {
                List<ICommonDbConnection> dbConnections = GetDbConnections(); 
                if (dbConnections.Count == 0)
                    throw new System.Exception("No database connections have been initialized"); 
                loggingWriters.Add(new DbLogWriter(dbConnections)); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<ICommonDbConnection> GetDbConnections()
        {
            List<ICommonDbConnection> list = new List<ICommonDbConnection>(); 
            try
            {
                list.Add(new SqliteDbConnection(System.IO.Path.Combine(SettingsHelper.GetRootFolder(), "data/app.db"))); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
            return list; 
        }
    }
}

using System.Collections.Generic; 
using System.Windows; 
using SqlViewer.Helpers; 
using SqlViewer.Models.Creational.Logging; 
using SqlViewerDatabase.DbConnections; 

namespace SqlViewer.Models.Logging
{
    /// <summary>
    /// Intermediatory class for writing logs 
    /// </summary>
    public class LoggingHub
    {
        /// <summary>
        /// The way of writing logs 
        /// </summary>
        public List<ILoggingWriter> LoggingWriters { get; private set; } = null; 
        /// <summary>
        /// Class for creating log writers 
        /// </summary>
        private ILoggingWriterCreator LoggingWriterCreator { get; set; } = new LoggingWriterCreator(); 

        /// <summary>
        /// Constructor of LoggingHub
        /// </summary>
        public LoggingHub() => InitLoggingWriter(); 

        /// <summary>
        /// Initializes a list of instances for writing logs 
        /// </summary>
        public void InitLoggingWriter() => LoggingWriters = LoggingWriterCreator.GetLoggingWriters(); 

        /// <summary>
        /// Initializes a list of instances for writing logs 
        /// </summary>
        public void WriteLog(string msg)
        {
            try
            {
                if (LoggingWriters.Count == 0)
                    throw new System.Exception("Unable to write a log message because the list of logging writers is empty"); 
                foreach (ILoggingWriter loggingWriter in LoggingWriters)
                {
                    loggingWriter.WriteLog(msg);
                }
            }
            catch(System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

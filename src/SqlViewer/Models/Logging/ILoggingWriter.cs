namespace SqlViewer.Models.Logging
{
    /// <summary>
    /// Interface for writing logs 
    /// </summary>
    public interface ILoggingWriter
    {
        void WriteLog(string msg);
    }
}

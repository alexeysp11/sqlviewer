namespace SqlViewer.Models.Logging
{
    public interface ILoggingWriter
    {
        void WriteLog(string msg);
    }
}

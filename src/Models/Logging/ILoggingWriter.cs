namespace SqlViewer.Models.Logging
{
    public interface ILoggingWriter
    {
        public void WriteLog(string msg);
    }
}

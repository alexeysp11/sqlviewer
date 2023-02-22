using System.Collections.Generic; 
using SqlViewer.Models.Logging; 

namespace SqlViewer.Models.Creational.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILoggingWriterCreator
    {
        List<ILoggingWriter> GetLoggingWriters(); 
    }
}

using System.Collections.Generic; 
using SqlViewer.Models.Logging; 

namespace SqlViewer.Models.Creational.Logging
{
    /// <summary>
    /// Interface for creating logging writers 
    /// </summary>
    public interface ILoggingWriterCreator
    {
        List<ILoggingWriter> GetLoggingWriters(); 
    }
}

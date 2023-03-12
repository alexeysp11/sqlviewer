using System.Data; 

namespace SqlViewer.Models.DbPreproc.DbDatabasePreproc
{
    /// <summary>
    /// Interface for database connection related operations
    /// </summary>
    public interface IDbDatabasePreproc
    {
        void CreateDb(); 
        void OpenDb(); 
        DataTable DisplayTablesInDb(); 
    }
}

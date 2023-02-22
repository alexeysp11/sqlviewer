using System.Data; 

namespace SqlViewer.Models.DbPreproc.DbDatabasePreproc
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDbDatabasePreproc
    {
        void CreateDb(); 
        void OpenDb(); 
        DataTable DisplayTablesInDb(); 
    }
}

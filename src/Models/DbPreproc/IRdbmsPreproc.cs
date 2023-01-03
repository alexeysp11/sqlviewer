using System.Data; 
using ICommonDbConnectionSV = SqlViewer.Models.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.DbPreproc
{
    public interface IRdbmsPreproc
    {
        void CreateDb();
        void OpenDb(); 

        void InitUserDbConnection(); 

        void DisplayTablesInDb(); 
        void GetAllDataFromTable(string tableName); 
        void GetColumnsOfTable(string tableName); 
        void GetForeignKeys(string tableName); 
        void GetTriggers(string tableName); 
        void GetSqlDefinition(string tableName); 

        void SendSqlRequest(); 
        DataTable SendSqlRequest(string sql); 
        void ClearTempTable(string tableName); 

        ICommonDbConnectionSV GetAppDbConnection(); 
        ICommonDbConnectionSV GetUserDbConnection(); 
    }
}

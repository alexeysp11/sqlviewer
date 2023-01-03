using System.Data;
using SqlViewer.ViewModels;  
using ICommonDbConnectionSV = SqlViewer.Models.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.DbPreproc
{
    public class MysqlDbPreproc : IRdbmsPreproc
    {
        private MainVM MainVM { get; set; }

        public ICommonDbConnectionSV AppDbConnection { get; private set; }
        public ICommonDbConnectionSV UserDbConnection { get; private set; }

        public MysqlDbPreproc(MainVM mainVM)
        {
            this.MainVM = mainVM; 
        }

        public void CreateDb()
        {

        }
        public void OpenDb()
        {

        } 

        public void InitUserDbConnection()
        {
            
        }

        public void DisplayTablesInDb()
        {

        } 
        public void GetAllDataFromTable(string tableName)
        {

        } 
        public void GetColumnsOfTable(string tableName)
        {

        } 
        public void GetForeignKeys(string tableName)
        {

        } 
        public void GetTriggers(string tableName)
        {

        } 
        public void GetSqlDefinition(string tableName)
        {

        } 

        public void SendSqlRequest()
        {

        } 
        public DataTable SendSqlRequest(string sql)
        {
            return new DataTable(); 
        } 
        public void ClearTempTable(string tableName)
        {

        } 

        public ICommonDbConnectionSV GetAppDbConnection()
        {
            return AppDbConnection; 
        }
        public ICommonDbConnectionSV GetUserDbConnection()
        {
            return UserDbConnection; 
        }
    }
}

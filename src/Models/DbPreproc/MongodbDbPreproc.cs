using System.Data;
using SqlViewer.ViewModels;  
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.DbPreproc
{
    public class MongodbDbPreproc : BaseDodbPreproc, IDbPreproc
    {
        private MainVM MainVM { get; set; }

        public MongodbDbPreproc(MainVM mainVM)
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
            return base.AppDbConnection; 
        }
        public ICommonDbConnectionSV GetUserDbConnection()
        {
            return base.UserDbConnection; 
        }
    }
}

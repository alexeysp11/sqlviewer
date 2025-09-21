using SqlViewer.Models.DataStorage; 

namespace SqlViewer.Helpers
{
    public static class JsonXmlHelper
    {
        public static string GetTableInfoRequestXml(string methodName, string tableName)
        {
            return "<GetTableInfoRequest><MethodName>" + methodName + "</MethodName><TableName>" + tableName + "</TableName></GetTableInfoRequest>"; 
        }

        public static string GetTableInfoResponseXml(string methodName, string tableName, string result)
        {
            return "<GetTableInfoResponse><MethodName>" + methodName + "</MethodName><TableName>" + tableName + "</TableName><Result>" + result + "</Result></GetTableInfoResponse>"; 
        }
    }
}

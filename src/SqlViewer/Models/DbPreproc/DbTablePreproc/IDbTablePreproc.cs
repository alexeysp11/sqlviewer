namespace SqlViewer.Models.DbPreproc.DbTablePreproc
{
    /// <summary>
    /// Interface for table related operations
    /// </summary>
    public interface IDbTablePreproc
    {
        string GetColumnsOfTableSql(string tableName); 
        string GetForeignKeysSql(string tableName); 
        string GetTriggersSql(string tableName); 
        string GetTableDefinitionSql(string tableName); 
    }
}

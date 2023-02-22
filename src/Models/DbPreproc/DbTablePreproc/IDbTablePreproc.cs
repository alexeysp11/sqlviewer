namespace SqlViewer.Models.DbPreproc.DbTablePreproc
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDbTablePreproc
    {
        string GetColumnsOfTableSql(string tableName); 
        string GetForeignKeysSql(string tableName); 
        string GetTriggersSql(string tableName); 
        string GetSqlDefinitionSql(string tableName); 
    }
}

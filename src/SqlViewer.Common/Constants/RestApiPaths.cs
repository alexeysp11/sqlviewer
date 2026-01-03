namespace SqlViewer.Common.Constants;

public static class RestApiPaths
{
    public const string SqlQuery = "/api/sql/query";
    public const string SqlExecute = "/api/sql/execute";
    public const string SqlCreateTable = "/api/sql/create-table";
    public const string SqlDropTable = "/api/sql/drop-table";

    public const string MetadataColumns = "/api/metadata/columns";
    public const string MetadataTables = "/api/metadata/tables";

    public const string GetDbProviderDocs = "/api/docs/db-providers";

    public const string QueryBuilderCreateTable = "/api/query-builder/create/table";
}

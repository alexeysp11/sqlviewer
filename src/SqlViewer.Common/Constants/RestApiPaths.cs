using System.Diagnostics.CodeAnalysis;

namespace SqlViewer.Common.Constants;

[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "<Pending>")]
public static class RestApiPaths
{
    private const string ApiBase = "/api";

    public static class Sql
    {
        private const string Base = $"{ApiBase}/sql";
        public const string Query = $"{Base}/query";
        public const string Execute = $"{Base}/execute";
        public const string CreateTable = $"{Base}/create-table";
        public const string DropTable = $"{Base}/drop-table";
    }

    public static class Metadata
    {
        private const string Base = $"{ApiBase}/metadata";
        public const string Columns = $"{Base}/columns";
        public const string Tables = $"{Base}/tables";
    }

    public static class Docs
    {
        private const string Base = $"{ApiBase}/docs";
        public const string DbProviders = $"{Base}/db-providers";
    }

    public static class QueryBuilder
    {
        private const string Base = $"{ApiBase}/query-builder";
        public const string CreateTable = $"{Base}/create/table";
    }
}

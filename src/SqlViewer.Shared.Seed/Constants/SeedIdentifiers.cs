namespace SqlViewer.Shared.Seed.Constants;

#pragma warning disable CA1034 // Nested types should not be visible
public static class SeedIdentifiers
{
    public static class User
    {
        public static class Uid
        {
            public static readonly Guid Admin = Guid.Parse("550e8400-e29b-41d4-a716-446655440000");
            public static readonly Guid System = Guid.Parse("550e8400-e29b-41d4-a716-446655440001");
            public static readonly Guid Guest = Guid.Parse("550e8400-e29b-41d4-a716-446655440002");
        }

        public static class Username
        {
            public const string Admin = "admin";
            public const string Guest = "guest";
        }

        public static class Password
        {
            public const string Default = "default";
            public const string System = "system";
        }
    }

    public static class DataSource
    {
        public static class Uid
        {
            public static readonly Guid Metadata = Guid.Parse("a1b2c3d4-e5f6-4a5b-bc6d-7e8f9a0b1c2d");
            public static readonly Guid Security = Guid.Parse("b2c3d4e5-f6a7-5b6c-cd7e-8f9a0b1c2d3e");
            public static readonly Guid QueryExecution = Guid.Parse("b2c3d4e5-f6a7-5b6c-cd7e-8f9a0b1c2d3d");
        }

        public static class Name
        {
            public const string Metadata = "pg-metadata-db";
            public const string Security = "pg-security-db";
            public const string QueryExecution = "pg-query-db";
        }

        public static class Description
        {
            public const string Metadata = "sqlviewer metadata database";
            public const string Security = "sqlviewer security database";
            public const string QueryExecution = "sqlviewer query database";
        }

        public static class ConnectionString
        {
            public const string Default = "default";
        }
    }

    public static class DataSourcePermission
    {
        public static class Uid
        {
            public static readonly Guid AdminToMetadata = Guid.Parse("550e8401-e29b-41d4-a716-446655440010");
            public static readonly Guid AdminToSecurity = Guid.Parse("550e8401-e29b-41d4-a716-446655440011");
            public static readonly Guid AdminToQueryExecution = Guid.Parse("550e8401-e29b-41d4-a716-446655440012");
        }

        public static class AccessLevel
        {
            public const string Undefined = "Undefined";
            public const string ReadOnly = "ReadOnly";
            public const string ReadWrite = "ReadWrite";
            public const string Admin = "Admin";
        }
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Guest = "Guest";
        public const string Service = "Service";
    }

    public static class DbType
    {
        public const string Postgres = "PostgreSQL";
    }
}
#pragma warning restore CA1034 // Nested types should not be visible

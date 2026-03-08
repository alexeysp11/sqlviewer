using System.Diagnostics.CodeAnalysis;

namespace SqlViewer.Common.Constants;

[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "<Pending>")]
public static class ConfigurationKeys
{
    public static class Encryption
    {
        private const string Section = "EncryptionSettings";

        public const string Key = $"{Section}:Key";
    }

    public static class Jwt
    {
        private const string Section = "Jwt";

        public const string Key = $"{Section}:Key";
        public const string ExpiryLifetimeMinutes = $"{Section}:ExpiryLifetimeMinutes";
        public const string Issuer = $"{Section}:Issuer";
        public const string Audience = $"{Section}:Audience";
        public const string IssuerSigningKey = $"{Section}:IssuerSigningKey";
    }

    public static class Services
    {
        private const string Root = "Services";

        public static class Grpc
        {
            private const string Section = $"{Root}:Grpc";

            public const string Security = $"{Section}:Security";
            public const string Metadata = $"{Section}:Metadata";
            public const string QueryExecution = $"{Section}:QueryExecution";
        }
    }

    public static class DefaultUserCredentials
    {
        private const string Root = "DefaultUserCredentials";

        public static class Username
        {
            private const string Section = $"{Root}:Username";

            public const string Admin = $"{Section}:Admin";
            public const string Guest = $"{Section}:Guest";
        }

        public static class Password
        {
            private const string Section = $"{Root}:Password";

            public const string Admin = $"{Section}:Admin";
            public const string Guest = $"{Section}:Guest";
        }
    }

    public static class DefaultDataSources
    {
        private const string Root = "DefaultDataSources";

        public static class DbName
        {
            private const string Section = $"{Root}:DbName";

            public const string Metadata = $"{Section}:Metadata";
            public const string Security = $"{Section}:Security";
            public const string QueryExecution = $"{Section}:QueryExecution";
        }

        public static class DbDescription
        {
            private const string Section = $"{Root}:DbDescription";

            public const string Metadata = $"{Section}:Metadata";
            public const string Security = $"{Section}:Security";
            public const string QueryExecution = $"{Section}:QueryExecution";
        }
    }

    public static class ConnectionStrings
    {
        public const string Metadata = "MetadataConnection";
        public const string Security = "SecurityConnection";
        public const string QueryExecution = "QueryExecutionConnection";
    }
}

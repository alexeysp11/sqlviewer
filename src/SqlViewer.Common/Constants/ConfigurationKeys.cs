using System.Diagnostics.CodeAnalysis;

namespace SqlViewer.Common.Constants;

[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "<Pending>")]
public static class ConfigurationKeys
{
    public static class Encryption
    {
        public const string Section = "EncryptionSettings";
        public const string Key = $"{Section}:Key";
    }

    public static class DefaultUserCredentials
    {
        public const string Section = "DefaultUserCredentials";
        public const string AdminUsername = $"{Section}:AdminUsername";
        public const string AdminPassword = $"{Section}:AdminPassword";
    }

    public static class DefaultDataSources
    {
        public const string Section = "DefaultDataSources";
        public const string MetadataDbName = $"{Section}:MetadataDbName";
        public const string MetadataDbDescription = $"{Section}:MetadataDbDescription";
    }

    public static class ConnectionStrings
    {
        public const string Metadata = "MetadataConnection";
    }
}

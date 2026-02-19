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
        }
    }

    public static class DataSource
    {
        public static class Uid
        {
            public static readonly Guid Metadata = Guid.Parse("a1b2c3d4-e5f6-4a5b-bc6d-7e8f9a0b1c2d");
            public static readonly Guid Security = Guid.Parse("b2c3d4e5-f6a7-5b6c-cd7e-8f9a0b1c2d3e");
        }
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Guest = "Guest";
        public const string Service = "Service";
    }
}
#pragma warning restore CA1034 // Nested types should not be visible

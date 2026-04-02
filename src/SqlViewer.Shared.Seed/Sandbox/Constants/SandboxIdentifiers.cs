namespace SqlViewer.Shared.Seed.Sandbox.Constants;

[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "<Pending>")]
public static class SandboxIdentifiers
{
    public static class Books
    {
        public const int CleanCode = 1;
        public const int Refactoring = 2;
    }

    public static class Authors
    {
        public const int Martin = 10;
        public const int Fowler = 11;
    }

    public static class Regions
    {
        public const int Central = 101;
        public const int Nordic = 102;
    }

    public static class ArchiveLink
    {
        public static int SourceId = 1;
        public static Guid TargetId = Guid.Parse("550e84e0-e29b-41d4-a716-44bb5554ff00");
        public static string LinkType = "Manual";
    }
}

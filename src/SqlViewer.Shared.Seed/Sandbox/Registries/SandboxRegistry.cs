using SqlViewer.Shared.Seed.Sandbox.Constants;
using SqlViewer.Shared.Seed.Sandbox.Entities.Archive;
using SqlViewer.Shared.Seed.Sandbox.Entities.Public;

namespace SqlViewer.Shared.Seed.Sandbox.Registries;

public static class SandboxRegistry
{
    public static IEnumerable<RegionEntity> Regions => [
        new() { Id = SandboxIdentifiers.Regions.Central, Name = "Central Europe", Timezone = "CET", TimezoneOffset = 1 },
        new() { Id = SandboxIdentifiers.Regions.Nordic, Name = "Nordic", Timezone = "EET", TimezoneOffset = 2 }
    ];

    public static IEnumerable<AuthorEntity> Authors => [
        new() { Id = SandboxIdentifiers.Authors.Martin, FullName = "Robert C. Martin" },
        new() { Id = SandboxIdentifiers.Authors.Fowler, FullName = "Martin Fowler" }
    ];

    public static IEnumerable<BookEntity> Books => [
        new() { Id = SandboxIdentifiers.Books.CleanCode, Title = "Clean Code", Isbn10 = "0132350882", YearOfPublication = 2008 },
        new() { Id = SandboxIdentifiers.Books.Refactoring, Title = "Refactoring", Isbn10 = "0201485672", YearOfPublication = 1999 }
    ];

    public static IEnumerable<DeletedUserRegistryEntity> DeletedUsers => [
        new() {
            UserId = 5001,
            DeletionReason = "Security Breach",
            DeletedAt = new DateOnly(2023, 05, 20),
            ProfileSnapshot = "{\"email\": \"hacker@test.com\", \"last_ip\": \"192.168.1.1\"}"
        },
        new() {
            UserId = 5002,
            DeletionReason = "GDPR Request",
            DeletedAt = new DateOnly(2024, 01, 10),
            ProfileSnapshot = "{\"email\": \"user@example.com\", \"marketing\": false}"
        }
    ];
}

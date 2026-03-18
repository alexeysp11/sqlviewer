using Microsoft.EntityFrameworkCore;
using SqlViewer.Metadata.Data.DbContexts;
using SqlViewer.Shared.Seed.Sandbox.Constants;
using SqlViewer.Shared.Seed.Sandbox.Entities.Archive;
using SqlViewer.Shared.Seed.Sandbox.Entities.Public;
using SqlViewer.Shared.Seed.Sandbox.Registries;

namespace SqlViewer.Metadata.Data.DataSeeding;

public sealed class SandboxDataSeeder(SandboxDbContext context) : ISandboxDataSeeder
{
    public async Task SeedAsync()
    {
        await context.Database.EnsureCreatedAsync();

        // 1. Regions
        await SeedTable(context.Regions, SandboxRegistry.Regions, x => x.Id);

        // 2. Authors and Books
        await SeedTable(context.Authors, SandboxRegistry.Authors, x => x.Id);
        await SeedTable(context.Books, SandboxRegistry.Books, x => x.Id);
        await context.SaveChangesAsync();

        // 3. Relationships (BookAuthor), define relationships deterministically.
        if (!await context.BookAuthors.AnyAsync())
        {
            context.BookAuthors.AddRange([
                new() { BookId = SandboxIdentifiers.Books.CleanCode, AuthorId = SandboxIdentifiers.Authors.Martin },
                new() { BookId = SandboxIdentifiers.Books.Refactoring, AuthorId = SandboxIdentifiers.Authors.Fowler }
            ]);
        }

        // 4. Logs and complex types (Audit, Tags)
        if (!await context.AuditLogs.AnyAsync())
        {
            context.AuditLogs.Add(new AuditLogEntity
            {
                UserData = "{\"event\": \"initial_seed\", \"status\": \"success\"}"
            });
        }

        // 5. Archived data (Deleted Users)
        await SeedTable(context.DeletedUserRegistries, SandboxRegistry.DeletedUsers, x => x.UserId);
        if (!await context.ArchiveLinks.AnyAsync())
        {
            context.ArchiveLinks.Add(new ArchiveLinkEntity
            {
                SourceId = SandboxIdentifiers.ArchiveLink.SourceId,
                TargetId = SandboxIdentifiers.ArchiveLink.TargetId,
                LinkType = SandboxIdentifiers.ArchiveLink.LinkType
            });
        }

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// A generic method for idempotent insertion (checks for presence by ID).
    /// </summary>
    private static async Task SeedTable<T>(DbSet<T> dbSet, IEnumerable<T> data, Func<T, object> idSelector) where T : class
    {
        foreach (T item in data)
        {
            object id = idSelector(item);
            if (await dbSet.FindAsync(id) == null)
            {
                dbSet.Add(item);
            }
        }
    }
}

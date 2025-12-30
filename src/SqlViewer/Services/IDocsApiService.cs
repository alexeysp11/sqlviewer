using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services;

public interface IDocsApiService : IDisposable
{
    Task<string> GetDbProviderDocs(VelocipedeDatabaseType databaseType);
}

using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services.Abstractions;

public interface IDocsApiService
{
    Task<string> GetDbProviderDocs(VelocipedeDatabaseType databaseType);
}

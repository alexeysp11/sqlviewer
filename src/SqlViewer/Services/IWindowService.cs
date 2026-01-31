using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services;

public interface IWindowService
{
    void ShowEtlWizard(
        ISqlApiService sqlApiService,
        IMetadataApiService metadataService,
        IQueryBuilderApiService queryBuilderService,
        string connectionString,
        VelocipedeDatabaseType databaseType);
}

using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services;

public interface IWindowService
{
    void ShowEtlTableStructureWizard(
        string connectionString,
        VelocipedeDatabaseType databaseType);

    void ShowEtlDataTransferWindow(
        string connectionString,
        VelocipedeDatabaseType databaseType);
}

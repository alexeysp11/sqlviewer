using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services;

public interface IWindowService
{
    void ShowEtlWizard(string connectionString, VelocipedeDatabaseType databaseType);
}


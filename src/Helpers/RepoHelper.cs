using SqlViewer.Models.DataStorage; 

namespace SqlViewer.Helpers
{
    public static class RepoHelper
    {
        public static AppSettingsRepo AppSettingsRepo { get; private set; } = null; 

        public static void SetAppSettingsRepo(AppSettingsRepo appSettingsRepo)
        {
            AppSettingsRepo = appSettingsRepo; 
        }
    }
}
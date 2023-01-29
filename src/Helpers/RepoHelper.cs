using SqlViewer.Models.DataStorage; 
using SqlViewer.Models.EnumOperations; 

namespace SqlViewer.Helpers
{
    public static class RepoHelper
    {
        public static AppSettingsRepo AppSettingsRepo { get; private set; } = null; 

        public static EnumEncoder EnumEncoder { get; private set; } = new EnumEncoder(); 
        public static EnumDecoder EnumDecoder { get; private set; } = new EnumDecoder(); 

        public static void SetAppSettingsRepo(AppSettingsRepo appSettingsRepo) => AppSettingsRepo = appSettingsRepo; 
    }
}

using SqlViewer.Models.EnumOperations; 
using LanguageEnum = SqlViewer.Enums.Common.Language; 
using AutoSaveEnum = SqlViewer.Enums.Editor.AutoSave; 
using FontSizeEnum = SqlViewer.Enums.Editor.FontSize; 
using FontFamilyEnum = SqlViewer.Enums.Editor.FontFamily; 
using TabSizeEnum = SqlViewer.Enums.Editor.TabSize; 
using WordWrapEnum = SqlViewer.Enums.Editor.WordWrap; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.DataStorage
{
    public class AppSettingsRepo
    {
        private EnumEncoder EnumEncoder {get; set; } 

        public LanguageEnum Language { get; private set; }
        public AutoSaveEnum AutoSave { get; private set; }
        public FontSizeEnum FontSize { get; private set; }
        public FontFamilyEnum FontFamily { get; private set; }
        public TabSizeEnum TabSize { get; private set; }
        public WordWrapEnum WordWrap { get; private set; }

        public RdbmsEnum DefaultRdbms { get; private set; }
        public RdbmsEnum ActiveRdbms { get; private set; }

        public string DbHost { get; private set; }
        public string DbPort { get; private set; }
        public string DbName { get; private set; }
        public string DbSchema { get; private set; }
        public string DbUsername { get; private set; }
        public string DbPassword { get; private set; }

        public AppSettingsRepo(EnumEncoder enumEncoder, string language, string autoSave, int fontSize, string fontFamily, 
            int tabSize, string wordWrap, string defaultRdbms, string activeRdbms)
        {
            try
            {
                this.EnumEncoder = enumEncoder; 
                AssignBasic(language, autoSave, fontSize, fontFamily, tabSize, wordWrap, defaultRdbms, activeRdbms); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        public AppSettingsRepo(EnumEncoder enumEncoder, string language, string autoSave, int fontSize, string fontFamily, 
            int tabSize, string wordWrap, string defaultRdbms, string activeRdbms, string server, string dbName, string port,
            string dbSchema, string dbUsername, string dbPassword)
        {
            try
            {
                this.EnumEncoder = enumEncoder; 
                AssignBasic(language, autoSave, fontSize, fontFamily, tabSize, wordWrap, defaultRdbms, activeRdbms); 
                AssignDbCredentials(server, dbName, port, dbSchema, dbUsername, dbPassword); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        private void AssignBasic(string language, string autoSave, int fontSize, string fontFamily, 
            int tabSize, string wordWrap, string defaultRdbms, string activeRdbms)
        {
            try
            {
                Language = this.EnumEncoder.GetLanguageEnum($"{language}"); 
                AutoSave = this.EnumEncoder.GetAutoSaveEnum($"{autoSave}"); 
                FontSize = this.EnumEncoder.GetFontSizeEnum($"{fontSize}"); 
                FontFamily = this.EnumEncoder.GetFontFamilyEnum($"{fontFamily}"); 
                TabSize = this.EnumEncoder.GetTabSizeEnum($"{tabSize}"); 
                WordWrap = this.EnumEncoder.GetWordWrapEnum($"{wordWrap}"); 
                DefaultRdbms = this.EnumEncoder.GetRdbmsEnum($"{defaultRdbms}"); 
                ActiveRdbms = this.EnumEncoder.GetRdbmsEnum($"{activeRdbms}"); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        private void AssignDbCredentials(string server, string dbName, string port, string dbSchema, string dbUsername, string dbPassword)
        {
            DbHost = server; 
            DbName = dbName; 
            DbPort = port; 
            DbSchema = dbSchema; 
            DbUsername = dbUsername; 
            DbPassword = dbPassword; 
        }

        public void Update(string language, string autoSave, int fontSize, string fontFamily, int tabSize, 
            string wordWrap, string defaultRdbms, string activeRdbms, string server, string dbName, string port, 
            string dbSchema, string dbUsername, string dbPassword)
        {
            try
            {
                AssignBasic(language, autoSave, fontSize, fontFamily, tabSize, wordWrap, defaultRdbms, activeRdbms); 
                AssignDbCredentials(server, dbName, port, dbSchema, dbUsername, dbPassword); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        public void SetActiveRdbms(string activeRdbms)
        {
            if (string.IsNullOrEmpty(activeRdbms))
            {
                throw new System.Exception("Parameter 'activeRdbms' should not be null or empty"); 
            }
            ActiveRdbms = this.EnumEncoder.GetRdbmsEnum($"{activeRdbms}"); 
        }

        public void SetDbName(string dbName)
        {
            if (string.IsNullOrEmpty(dbName))
            {
                throw new System.Exception("Parameter 'dbName' should not be null or empty"); 
            }
            DbName = dbName; 
        }
    }
}

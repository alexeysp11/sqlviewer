using SqlViewer.Models.EnumOperations; 
using LanguageEnum = SqlViewer.Enums.Common.Language; 
using AutoSaveEnum = SqlViewer.Enums.Editor.AutoSave; 
using FontSizeEnum = SqlViewer.Enums.Editor.FontSize; 
using FontFamilyEnum = SqlViewer.Enums.Editor.FontFamily; 
using TabSizeEnum = SqlViewer.Enums.Editor.TabSize; 
using WordWrapEnum = SqlViewer.Enums.Editor.WordWrap; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models
{
    public class AppRepository
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

        public System.String DbName { get; private set; }
        public System.String DbSchema { get; private set; }
        public System.String DbUsername { get; private set; }
        public System.String DbPassword { get; private set; }

        public AppRepository(EnumEncoder enumEncoder, System.String language, System.String autoSave, System.Int32 fontSize, System.String fontFamily, 
            System.Int32 tabSize, System.String wordWrap, System.String defaultRdbms, System.String activeRdbms)
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

        public AppRepository(EnumEncoder enumEncoder, System.String language, System.String autoSave, System.Int32 fontSize, System.String fontFamily, 
            System.Int32 tabSize, System.String wordWrap, System.String defaultRdbms, System.String activeRdbms, System.String dbName, System.String dbSchema, 
            System.String dbUsername, System.String dbPassword)
        {
            try
            {
                this.EnumEncoder = enumEncoder; 
                AssignBasic(language, autoSave, fontSize, fontFamily, tabSize, wordWrap, defaultRdbms, activeRdbms); 
                AssignDbCredentials(dbName, dbSchema, dbUsername, dbPassword); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        private void AssignBasic(System.String language, System.String autoSave, System.Int32 fontSize, System.String fontFamily, 
            System.Int32 tabSize, System.String wordWrap, System.String defaultRdbms, System.String activeRdbms)
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

        private void AssignDbCredentials(System.String dbName, System.String dbSchema, System.String dbUsername, System.String dbPassword)
        {
            DbName = dbName; 
            DbSchema = dbSchema; 
            DbUsername = dbUsername; 
            DbPassword = dbPassword; 
        }

        public void Update(System.String language, System.String autoSave, System.Int32 fontSize, System.String fontFamily, System.Int32 tabSize, 
            System.String wordWrap, System.String defaultRdbms, System.String activeRdbms, System.String dbName, System.String dbSchema, 
            System.String dbUsername, System.String dbPassword)
        {
            try
            {
                AssignBasic(language, autoSave, fontSize, fontFamily, tabSize, wordWrap, defaultRdbms, activeRdbms); 
                AssignDbCredentials(dbName, dbSchema, dbUsername, dbPassword); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
    }
}
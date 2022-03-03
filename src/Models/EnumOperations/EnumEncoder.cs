using LanguageEnum = SqlViewer.Enums.Common.Language; 
using AutoSaveEnum = SqlViewer.Enums.Editor.AutoSave; 
using FontSizeEnum = SqlViewer.Enums.Editor.FontSize; 
using FontFamilyEnum = SqlViewer.Enums.Editor.FontFamily; 
using TabSizeEnum = SqlViewer.Enums.Editor.TabSize; 
using WordWrapEnum = SqlViewer.Enums.Editor.WordWrap; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.EnumOperations
{
    public class EnumEncoder
    {
        #region Common 
        public LanguageEnum GetLanguageEnum(string language)
        {
            if (language == "English")
                return LanguageEnum.English; 
            else if (language == "German")
                return LanguageEnum.German; 
            else if (language == "Russian")
                return LanguageEnum.Russian; 
            else if (language == "Spanish")
                return LanguageEnum.Spanish; 
            else if (language == "Portugues")
                return LanguageEnum.Portugues; 
            else if (language == "Italian")
                return LanguageEnum.Italian; 
            else if (language == "French")
                return LanguageEnum.French; 
            else if (language == "Ukranian")
                return LanguageEnum.Ukranian; 
            else if (language == "Dutch")
                return LanguageEnum.Dutch; 
            else
                throw new System.Exception("Unable to find language enum"); 
        }
        #endregion  // Common 

        #region Editor
        public AutoSaveEnum GetAutoSaveEnum(string autoSave)
        {
            if (autoSave == "Enabled")
                return AutoSaveEnum.Enabled; 
            else if (autoSave == "Disabled")
                return AutoSaveEnum.Disabled; 
            else
                throw new System.Exception("Unable to find autoSave enum"); 
        }

        public FontSizeEnum GetFontSizeEnum(string fontSize)
        {
            if (fontSize == "8")
                return FontSizeEnum.FontSize8; 
            else if (fontSize == "9")
                return FontSizeEnum.FontSize9; 
            else if (fontSize == "10")
                return FontSizeEnum.FontSize10; 
            else if (fontSize == "11")
                return FontSizeEnum.FontSize11; 
            else if (fontSize == "12")
                return FontSizeEnum.FontSize12; 
            else if (fontSize == "14")
                return FontSizeEnum.FontSize14; 
            else if (fontSize == "16")
                return FontSizeEnum.FontSize16; 
            else if (fontSize == "18")
                return FontSizeEnum.FontSize18; 
            else if (fontSize == "20")
                return FontSizeEnum.FontSize20; 
            else
                throw new System.Exception("Unable to find fontSize enum"); 
        }

        public FontFamilyEnum GetFontFamilyEnum(string fontFamily)
        {
            if (fontFamily == "Consolas")
                return FontFamilyEnum.Consolas; 
            else
                throw new System.Exception("Unable to find fontFamily enum"); 
        }

        public TabSizeEnum GetTabSizeEnum(string tabSize)
        {
            if (tabSize == "1")
                return TabSizeEnum.TabSize1; 
            else if (tabSize == "2")
                return TabSizeEnum.TabSize2; 
            else if (tabSize == "3")
                return TabSizeEnum.TabSize3; 
            else if (tabSize == "4")
                return TabSizeEnum.TabSize4; 
            else if (tabSize == "5")
                return TabSizeEnum.TabSize5; 
            else if (tabSize == "6")
                return TabSizeEnum.TabSize6; 
            else if (tabSize == "7")
                return TabSizeEnum.TabSize7; 
            else if (tabSize == "8")
                return TabSizeEnum.TabSize8; 
            else
                throw new System.Exception("Unable to find tabSize enum"); 
        }

        public WordWrapEnum GetWordWrapEnum(string wordWrap)
        {
            if (wordWrap == "Enabled")
                return WordWrapEnum.Enabled; 
            else if (wordWrap == "Disabled")
                return WordWrapEnum.Disabled; 
            else
                throw new System.Exception("Unable to find wordWrap enum"); 
        }
        #endregion  // Editor

        #region Database 
        public RdbmsEnum GetRdbmsEnum(string rdbms)
        {
            if (rdbms == "SQLite")
                return RdbmsEnum.SQLite; 
            else if (rdbms == "PostgreSQL")
                return RdbmsEnum.PostgreSQL; 
            else if (rdbms == "MySQL")
                return RdbmsEnum.MySQL; 
            else
                throw new System.Exception("Unable to find RDBMS enum"); 
        }
        #endregion  // Database 
    }
}
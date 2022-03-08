using LanguageEnum = SqlViewer.Enums.Common.Language; 
using AutoSaveEnum = SqlViewer.Enums.Editor.AutoSave; 
using FontSizeEnum = SqlViewer.Enums.Editor.FontSize; 
using FontFamilyEnum = SqlViewer.Enums.Editor.FontFamily; 
using TabSizeEnum = SqlViewer.Enums.Editor.TabSize; 
using WordWrapEnum = SqlViewer.Enums.Editor.WordWrap; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.EnumOperations
{
    public class EnumDecoder
    {
        #region Common 
        public System.String GetLanguageName(LanguageEnum language)
        {
            if (language == LanguageEnum.English)
                return "English"; 
            else if (language == LanguageEnum.German)
                return "German"; 
            else if (language == LanguageEnum.Russian)
                return "Russian"; 
            else if (language == LanguageEnum.Spanish)
                return "Spanish"; 
            else if (language == LanguageEnum.Portugues)
                return "Portugues"; 
            else if (language == LanguageEnum.Italian)
                return "Italian"; 
            else if (language == LanguageEnum.French)
                return "French"; 
            else if (language == LanguageEnum.Ukranian)
                return "Ukranian"; 
            else if (language == LanguageEnum.Dutch)
                return "Dutch"; 
            else
                throw new System.Exception("Unable to find language name"); 
        }
        #endregion  // Common 

        #region Editor
        public System.String GetAutoSaveName(AutoSaveEnum autoSave)
        {
            if (autoSave == AutoSaveEnum.Enabled)
                return "Enabled"; 
            else if (autoSave == AutoSaveEnum.Disabled)
                return "Disabled"; 
            else
                throw new System.Exception("Unable to find autoSave name"); 
        }

        public System.String GetFontSizeName(FontSizeEnum fontSize)
        {
            if (fontSize == FontSizeEnum.FontSize8)
                return "8"; 
            else if (fontSize == FontSizeEnum.FontSize9)
                return "9"; 
            else if (fontSize == FontSizeEnum.FontSize10)
                return "10"; 
            else if (fontSize == FontSizeEnum.FontSize11)
                return "11"; 
            else if (fontSize == FontSizeEnum.FontSize12)
                return "12"; 
            else if (fontSize == FontSizeEnum.FontSize14)
                return "14"; 
            else if (fontSize == FontSizeEnum.FontSize16)
                return "16"; 
            else if (fontSize == FontSizeEnum.FontSize18)
                return "18"; 
            else if (fontSize == FontSizeEnum.FontSize20)
                return "20"; 
            else
                throw new System.Exception("Unable to find fontSize name"); 
        }

        public System.String GetFontFamilyName(FontFamilyEnum fontFamily)
        {
            if (fontFamily == FontFamilyEnum.Consolas)
                return "Consolas"; 
            else
                throw new System.Exception("Unable to find fontFamily name"); 
        }

        public System.String GetTabSizeName(TabSizeEnum tabSize)
        {
            if (tabSize == TabSizeEnum.TabSize1)
                return "1"; 
            else if (tabSize == TabSizeEnum.TabSize2)
                return "2"; 
            else if (tabSize == TabSizeEnum.TabSize3)
                return "3"; 
            else if (tabSize == TabSizeEnum.TabSize4)
                return "4"; 
            else if (tabSize == TabSizeEnum.TabSize5)
                return "5"; 
            else if (tabSize == TabSizeEnum.TabSize6)
                return "6"; 
            else if (tabSize == TabSizeEnum.TabSize7)
                return "7"; 
            else if (tabSize == TabSizeEnum.TabSize8)
                return "8"; 
            else
                throw new System.Exception("Unable to find tabSize name"); 
        }

        public System.String GetWordWrapName(WordWrapEnum wordWrap)
        {
            if (wordWrap == WordWrapEnum.Enabled)
                return "Enabled"; 
            else if (wordWrap == WordWrapEnum.Disabled)
                return "Disabled"; 
            else
                throw new System.Exception("Unable to find wordWrap name"); 
        }
        #endregion  // Editor

        #region Database 
        public System.String GetRdbmsName(RdbmsEnum rdbms)
        {
            if (rdbms == RdbmsEnum.SQLite)
                return "SQLite"; 
            else if (rdbms == RdbmsEnum.PostgreSQL)
                return "PostgreSQL"; 
            else if (rdbms == RdbmsEnum.MySQL)
                return "MySQL"; 
            else
                throw new System.Exception("Unable to find RDBMS name"); 
        }
        #endregion  // Database 
    }
}
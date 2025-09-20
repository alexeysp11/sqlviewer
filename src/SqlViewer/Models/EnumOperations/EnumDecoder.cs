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
        public string GetLanguageName(LanguageEnum language) => language switch
        {
            LanguageEnum.English       => "English",
            LanguageEnum.German        => "German",
            LanguageEnum.Russian       => "Russian",
            LanguageEnum.Spanish       => "Spanish",
            LanguageEnum.Portuguese    => "Portuguese",
            LanguageEnum.Italian       => "Italian",
            LanguageEnum.French        => "French",
            LanguageEnum.Ukranian      => "Ukranian",
            LanguageEnum.Dutch         => "Dutch",
            LanguageEnum.Polish        => "Polish",
            LanguageEnum.Czech         => "Czech",
            LanguageEnum.Serbian       => "Serbian",
            LanguageEnum.Croatian      => "Croatian",
            LanguageEnum.Swedish       => "Swedish",
            LanguageEnum.Norwegian     => "Norwegian",
            LanguageEnum.Danish        => "Danish",
            LanguageEnum.Afrikaans     => "Afrikaans",
            LanguageEnum.Turkish       => "Turkish",
            LanguageEnum.Kazakh        => "Kazakh",
            LanguageEnum.Armenian      => "Armenian",
            LanguageEnum.Georgian      => "Georgian",
            LanguageEnum.Romanian      => "Romanian",
            LanguageEnum.Bulgarian     => "Bulgarian",
            LanguageEnum.Albanian      => "Albanian",
            LanguageEnum.Greek         => "Greek",
            LanguageEnum.Indonesian    => "Indonesian",
            LanguageEnum.Malay         => "Malay",
            LanguageEnum.Korean        => "Korean",
            LanguageEnum.Japanese      => "Japanese",
            _ => throw new System.Exception("Unable to find language name")
        }; 
        #endregion  // Common 

        #region Editor
        public string GetAutoSaveName(AutoSaveEnum autoSave) => autoSave switch
        {
            AutoSaveEnum.Enabled   => "Enabled",
            AutoSaveEnum.Disabled  => "Disabled",
            _ => throw new System.Exception("Unable to find autoSave name")
        }; 

        public string GetFontSizeName(FontSizeEnum fontSize) => fontSize switch
        {
            FontSizeEnum.FontSize8     => "8",
            FontSizeEnum.FontSize9     => "9",
            FontSizeEnum.FontSize10    => "10",
            FontSizeEnum.FontSize11    => "11",
            FontSizeEnum.FontSize12    => "12",
            FontSizeEnum.FontSize14    => "14",
            FontSizeEnum.FontSize16    => "16",
            FontSizeEnum.FontSize18    => "18",
            FontSizeEnum.FontSize20    => "20",
            _ => throw new System.Exception("Unable to find fontSize name")
        }; 

        public string GetFontFamilyName(FontFamilyEnum fontFamily) => fontFamily switch
        {
            FontFamilyEnum.Consolas => "Consolas",
            _ => throw new System.Exception("Unable to find fontFamily name")
        }; 

        public string GetTabSizeName(TabSizeEnum tabSize) => tabSize switch
        {
            TabSizeEnum.TabSize1 => "1",
            TabSizeEnum.TabSize2 => "2",
            TabSizeEnum.TabSize3 => "3",
            TabSizeEnum.TabSize4 => "4",
            TabSizeEnum.TabSize5 => "5",
            TabSizeEnum.TabSize6 => "6",
            TabSizeEnum.TabSize7 => "7",
            TabSizeEnum.TabSize8 => "8",
            _ => throw new System.Exception("Unable to find tabSize name")
        }; 

        public string GetWordWrapName(WordWrapEnum wordWrap) => wordWrap switch
        {
            WordWrapEnum.Enabled   => "Enabled",
            WordWrapEnum.Disabled  => "Disabled",
            _ => throw new System.Exception("Unable to find wordWrap name")
        }; 
        #endregion  // Editor

        #region Database 
        public string GetRdbmsName(RdbmsEnum rdbms) => rdbms switch
        {
            RdbmsEnum.SQLite        => "SQLite",
            RdbmsEnum.PostgreSQL    => "PostgreSQL",
            RdbmsEnum.MySQL         => "MySQL",
            RdbmsEnum.Oracle        => "Oracle",
            _ => throw new System.Exception("Unable to find rdbms name")
        }; 
        #endregion  // Database 
    }
}

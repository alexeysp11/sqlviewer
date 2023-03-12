using LanguageEnum = SqlViewer.Enums.Common.Language; 
using AutoSaveEnum = SqlViewer.Enums.Editor.AutoSave; 
using FontSizeEnum = SqlViewer.Enums.Editor.FontSize; 
using FontFamilyEnum = SqlViewer.Enums.Editor.FontFamily; 
using TabSizeEnum = SqlViewer.Enums.Editor.TabSize; 
using WordWrapEnum = SqlViewer.Enums.Editor.WordWrap; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.EnumOperations
{
    /// <summary>
    /// Class for getting enums from string 
    /// </summary>  
    public class EnumEncoder
    {
        #region Common 
        /// <summary>
        /// 
        /// </summary>
        public LanguageEnum GetLanguageEnum(string language) => language switch
        {
            "English"       => LanguageEnum.English,
            "German"        => LanguageEnum.German,
            "Russian"       => LanguageEnum.Russian,
            "Spanish"       => LanguageEnum.Spanish,
            "Portuguese"    => LanguageEnum.Portuguese,
            "Italian"       => LanguageEnum.Italian,
            "French"        => LanguageEnum.French,
            "Ukranian"      => LanguageEnum.Ukranian,
            "Dutch"         => LanguageEnum.Dutch,
            "Polish"        => LanguageEnum.Polish,
            "Czech"         => LanguageEnum.Czech,
            "Serbian"       => LanguageEnum.Serbian,
            "Croatian"      => LanguageEnum.Croatian,
            "Swedish"       => LanguageEnum.Swedish,
            "Norwegian"     => LanguageEnum.Norwegian,
            "Danish"        => LanguageEnum.Danish,
            "Afrikaans"     => LanguageEnum.Afrikaans,
            "Turkish"       => LanguageEnum.Turkish,
            "Kazakh"        => LanguageEnum.Kazakh,
            "Armenian"      => LanguageEnum.Armenian,
            "Georgian"      => LanguageEnum.Georgian,
            "Romanian"      => LanguageEnum.Romanian,
            "Bulgarian"     => LanguageEnum.Bulgarian,
            "Albanian"      => LanguageEnum.Albanian,
            "Greek"         => LanguageEnum.Greek,
            "Indonesian"    => LanguageEnum.Indonesian,
            "Malay"         => LanguageEnum.Malay,
            "Korean"        => LanguageEnum.Korean,
            "Japanese"      => LanguageEnum.Japanese,
            _ => throw new System.Exception("Unable to find language enum")
        }; 
        #endregion  // Common 

        #region Editor
        /// <summary>
        /// 
        /// </summary>
        public AutoSaveEnum GetAutoSaveEnum(string autoSave) => autoSave switch
        {
            "Enabled"   => AutoSaveEnum.Enabled,
            "Disabled"  => AutoSaveEnum.Disabled,
            _ => throw new System.Exception("Unable to find autoSave enum")
        }; 

        /// <summary>
        /// 
        /// </summary>
        public FontSizeEnum GetFontSizeEnum(string fontSize) => fontSize switch
        {
            "8"     => FontSizeEnum.FontSize8,
            "9"     => FontSizeEnum.FontSize9,
            "10"    => FontSizeEnum.FontSize10,
            "11"    => FontSizeEnum.FontSize11,
            "12"    => FontSizeEnum.FontSize12,
            "14"    => FontSizeEnum.FontSize14,
            "16"    => FontSizeEnum.FontSize16,
            "18"    => FontSizeEnum.FontSize18,
            "20"    => FontSizeEnum.FontSize20,
            _ => throw new System.Exception("Unable to find fontSize enum")
        }; 

        /// <summary>
        /// 
        /// </summary>
        public FontFamilyEnum GetFontFamilyEnum(string fontFamily) => fontFamily switch
        {
            "Consolas" => FontFamilyEnum.Consolas,
            _ => throw new System.Exception("Unable to find fontFamily enum")
        }; 

        /// <summary>
        /// 
        /// </summary>
        public TabSizeEnum GetTabSizeEnum(string tabSize) => tabSize switch
        {
            "1" => TabSizeEnum.TabSize1,
            "2" => TabSizeEnum.TabSize2,
            "3" => TabSizeEnum.TabSize3,
            "4" => TabSizeEnum.TabSize4,
            "5" => TabSizeEnum.TabSize5,
            "6" => TabSizeEnum.TabSize6,
            "7" => TabSizeEnum.TabSize7,
            "8" => TabSizeEnum.TabSize8,
            _ => throw new System.Exception("Unable to find tabSize enum")
        }; 

        /// <summary>
        /// 
        /// </summary>
        public WordWrapEnum GetWordWrapEnum(string wordWrap) => wordWrap switch
        {
            "Enabled"   => WordWrapEnum.Enabled,
            "Disabled"  => WordWrapEnum.Disabled,
            _ => throw new System.Exception("Unable to find wordWrap enum")
        }; 
        #endregion  // Editor

        #region Database 
        /// <summary>
        /// 
        /// </summary>
        public RdbmsEnum GetRdbmsEnum(string rdbms) => rdbms switch
        {
            "SQLite"        => RdbmsEnum.SQLite,
            "PostgreSQL"    => RdbmsEnum.PostgreSQL,
            "MySQL"         => RdbmsEnum.MySQL,
            "Oracle"        => RdbmsEnum.Oracle,
            _ => throw new System.Exception("Unable to find rdbms enum")
        }; 
        #endregion  // Database 
    }
}

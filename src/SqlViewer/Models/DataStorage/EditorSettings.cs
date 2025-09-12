using SqlViewer.Helpers; 
using AutoSaveEnum = SqlViewer.Enums.Editor.AutoSave; 
using FontSizeEnum = SqlViewer.Enums.Editor.FontSize; 
using FontFamilyEnum = SqlViewer.Enums.Editor.FontFamily; 
using TabSizeEnum = SqlViewer.Enums.Editor.TabSize; 
using WordWrapEnum = SqlViewer.Enums.Editor.WordWrap; 

namespace SqlViewer.Models.DataStorage
{
    /// <summary>
    /// Class for storing editor related settings
    /// </summary>
    public class EditorSettings
    {
        #region Properties
        /// <summary>
        /// Parameter AutoSave for editor
        /// </summary>
        public AutoSaveEnum AutoSave { get; private set; }
        /// <summary>
        /// Parameter FontSize for editor
        /// </summary>
        public FontSizeEnum FontSize { get; private set; }
        /// <summary>
        /// Parameter FontFamily for editor
        /// </summary>
        public FontFamilyEnum FontFamily { get; private set; }
        /// <summary>
        /// Parameter TabSize for editor
        /// </summary>
        public TabSizeEnum TabSize { get; private set; }
        /// <summary>
        /// Parameter WordWrap for editor
        /// </summary>
        public WordWrapEnum WordWrap { get; private set; }
        #endregion  // Properties

        #region Public methods
        /// <summary>
        /// Sets parameter AutoSave for editor
        /// </summary>
        public void SetAutoSave(string autoSave) 
        {
            if (string.IsNullOrEmpty(autoSave)) throw new System.Exception("Parameter 'autoSave' could not be null or empty"); 
            AutoSave = RepoHelper.EnumEncoder.GetAutoSaveEnum(autoSave); 
        }
        /// <summary>
        /// Sets parameter FontSize for editor
        /// </summary>
        public void SetFontSize(int fontSize) 
        {
            FontSize = RepoHelper.EnumEncoder.GetFontSizeEnum(fontSize.ToString()); 
        }
        /// <summary>
        /// Sets parameter FontFamily for editor
        /// </summary>
        public void SetFontFamily(string fontFamily) 
        {
            if (string.IsNullOrEmpty(fontFamily)) throw new System.Exception("Parameter 'fontFamily' could not be null or empty"); 
            FontFamily = RepoHelper.EnumEncoder.GetFontFamilyEnum(fontFamily); 
        }
        /// <summary>
        /// Sets parameter TabSize for editor
        /// </summary>
        public void SetTabSize(int tabSize) 
        {
            TabSize = RepoHelper.EnumEncoder.GetTabSizeEnum(tabSize.ToString()); 
        }
        /// <summary>
        /// Sets parameter WordWrap for editor
        /// </summary>
        public void SetWordWrap(string wordWrap) 
        {
            if (string.IsNullOrEmpty(wordWrap)) throw new System.Exception("Parameter 'wordWrap' could not be null or empty"); 
            WordWrap = RepoHelper.EnumEncoder.GetWordWrapEnum(wordWrap); 
        }
        #endregion  // Public methods
    }
}

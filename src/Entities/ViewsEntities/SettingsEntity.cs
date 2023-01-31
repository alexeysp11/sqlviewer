using SqlViewer.Entities.Language;

namespace SqlViewer.Entities.ViewsEntities
{
    /// <summary>
    /// 
    /// </summary>
    public class SettingsEntity 
    {
        #region Editor fields
        /// <summary>
        /// 
        /// </summary>
        public Word EditorField { get; private set; } = new Word("Editor"); 
        /// <summary>
        /// 
        /// </summary>
        public Word LanguageField { get; private set; } = new Word("Language"); 
        /// <summary>
        /// 
        /// </summary>
        public Word AutoSaveField { get; private set; } = new Word("Auto save"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FontSizeField { get; private set; } = new Word("Font size"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FontFamilyField { get; private set; } = new Word("Font family"); 
        /// <summary>
        /// 
        /// </summary>
        public Word TabSizeField { get; private set; } = new Word("Tab size"); 
        /// <summary>
        /// 
        /// </summary>
        public Word WordWrapField { get; private set; } = new Word("Word wrap"); 
        #endregion  // Editor fields

        #region DB fields
        /// <summary>
        /// 
        /// </summary>
        public Word DbField { get; private set; } = new Word("DB"); 
        /// <summary>
        /// 
        /// </summary>
        public Word DefaultRdbmsField { get; private set; } = new Word("Default RDBMS"); 
        /// <summary>
        /// 
        /// </summary>
        public Word ActiveRdbmsField { get; private set; } = new Word("Active RDBMS"); 
        /// <summary>
        /// 
        /// </summary>
        public Word DatabaseField { get; private set; } = new Word("Database"); 
        /// <summary>
        /// 
        /// </summary>
        public Word SchemaField { get; private set; } = new Word("Schema"); 
        /// <summary>
        /// 
        /// </summary>
        public Word UsernameField { get; private set; } = new Word("Username"); 
        /// <summary>
        /// 
        /// </summary>
        public Word PasswordField { get; private set; } = new Word("Password"); 
        #endregion  // DB fields

        #region Common fields
        /// <summary>
        /// 
        /// </summary>
        public Word EnabledField { get; private set; } = new Word("Enabled"); 
        /// <summary>
        /// 
        /// </summary>
        public Word DisabledField { get; private set; } = new Word("Disabled"); 
        #endregion  // Common fields

        #region Common fields
        /// <summary>
        /// 
        /// </summary>
        public Word ChosenLanguageField { get; private set; } = new Word("Language"); 
        #endregion  // Common fields

        #region Buttons fields
        /// <summary>
        /// 
        /// </summary>
        public Word RecoverField { get; private set; } = new Word("Recover"); 
        /// <summary>
        /// 
        /// </summary>
        public Word SaveField { get; private set; } = new Word("Save"); 
        /// <summary>
        /// 
        /// </summary>
        public Word CancelField { get; private set; } = new Word("Cancel"); 
        #endregion  // Buttons fields

        #region Editor methods
        /// <summary>
        /// 
        /// </summary>
        public void TranslateEditorField(string value) => EditorField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateLanguageField(string value) => LanguageField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateAutoSaveField(string value) => AutoSaveField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFontSizeField(string value) => FontSizeField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFontFamilyField(string value) => FontFamilyField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateTabSizeField(string value) => TabSizeField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateWordWrapField(string value) => WordWrapField.SetTranslation(value); 
        #endregion  // Editor methods

        #region DB methods
        /// <summary>
        /// 
        /// </summary>
        public void TranslateDbField(string value) => DbField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateDefaultRdbmsField(string value) => DefaultRdbmsField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateActiveRdbmsField(string value) => ActiveRdbmsField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateDatabaseField(string value) => DatabaseField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateSchemaField(string value) => SchemaField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateUsernameField(string value) => UsernameField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslatePasswordField(string value) => PasswordField.SetTranslation(value); 
        #endregion  // DB methods

        #region Common field
        /// <summary>
        /// 
        /// </summary>
        public void TranslateEnabledField(string value) => EnabledField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateDisabledField(string value) => DisabledField.SetTranslation(value); 
        #endregion  // Common field

        #region Common field
        /// <summary>
        /// 
        /// </summary>
        public void TranslateChosenLanguageField(string value) => ChosenLanguageField.SetTranslation(value); 
        #endregion  // Common field

        #region Buttons methods
        /// <summary>
        /// 
        /// </summary>
        public void TranslateRecoverField(string value) => RecoverField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateSaveField(string value) => SaveField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateCancelField(string value) => CancelField.SetTranslation(value); 
        #endregion  // Buttons methods
    }
}
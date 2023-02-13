using SqlViewer.Entities.Language;

namespace SqlViewer.Entities.ViewsEntities
{
    /// <summary>
    /// Represents an entity of translating the settings page 
    /// </summary>
    public class SettingsEntity 
    {
        #region Editor fields
        /// <summary>
        /// Word 'Editor' and its translation on the settings page
        /// </summary>
        public Word EditorField { get; private set; } = new Word("Editor"); 
        /// <summary>
        /// Word 'Language' and its translation on the settings page
        /// </summary>
        public Word LanguageField { get; private set; } = new Word("Language"); 
        /// <summary>
        /// Word 'AutoSave' and its translation on the settings page
        /// </summary>
        public Word AutoSaveField { get; private set; } = new Word("Auto save"); 
        /// <summary>
        /// Word 'FontSize' and its translation on the settings page
        /// </summary>
        public Word FontSizeField { get; private set; } = new Word("Font size"); 
        /// <summary>
        /// Word 'FontFamily' and its translation on the settings page
        /// </summary>
        public Word FontFamilyField { get; private set; } = new Word("Font family"); 
        /// <summary>
        /// Word 'TabSize' and its translation on the settings page
        /// </summary>
        public Word TabSizeField { get; private set; } = new Word("Tab size"); 
        /// <summary>
        /// Word 'WordWrap' and its translation on the settings page
        /// </summary>
        public Word WordWrapField { get; private set; } = new Word("Word wrap"); 
        #endregion  // Editor fields

        #region DB fields
        /// <summary>
        /// Word 'Db' and its translation on the settings page
        /// </summary>
        public Word DbField { get; private set; } = new Word("DB"); 
        /// <summary>
        /// Word 'DefaultRdbms' and its translation on the settings page
        /// </summary>
        public Word DefaultRdbmsField { get; private set; } = new Word("Default RDBMS"); 
        /// <summary>
        /// Word 'ActiveRdbms' and its translation on the settings page
        /// </summary>
        public Word ActiveRdbmsField { get; private set; } = new Word("Active RDBMS"); 
        /// <summary>
        /// Word 'Database' and its translation on the settings page
        /// </summary>
        public Word DatabaseField { get; private set; } = new Word("Database"); 
        /// <summary>
        /// Word 'Port' and its translation on the settings page
        /// </summary>
        public Word PortField { get; private set; } = new Word("Port"); 
        /// <summary>
        /// Word 'Schema' and its translation on the settings page
        /// </summary>
        public Word SchemaField { get; private set; } = new Word("Schema"); 
        /// <summary>
        /// Word 'Username' and its translation on the settings page
        /// </summary>
        public Word UsernameField { get; private set; } = new Word("Username"); 
        /// <summary>
        /// Word 'Password' and its translation on the settings page
        /// </summary>
        public Word PasswordField { get; private set; } = new Word("Password"); 
        #endregion  // DB fields

        #region Common fields
        /// <summary>
        /// Word 'Enabled' and its translation on the settings page
        /// </summary>
        public Word EnabledField { get; private set; } = new Word("Enabled"); 
        /// <summary>
        /// Word 'Disabled' and its translation on the settings page
        /// </summary>
        public Word DisabledField { get; private set; } = new Word("Disabled"); 
        #endregion  // Common fields

        #region Common fields
        /// <summary>
        /// Word 'ChosenLanguage' and its translation on the settings page
        /// </summary>
        public Word ChosenLanguageField { get; private set; } = new Word("Language"); 
        #endregion  // Common fields

        #region Buttons fields
        /// <summary>
        /// Word 'Recover' and its translation on the settings page
        /// </summary>
        public Word RecoverField { get; private set; } = new Word("Recover"); 
        /// <summary>
        /// Word 'Save' and its translation on the settings page
        /// </summary>
        public Word SaveField { get; private set; } = new Word("Save"); 
        /// <summary>
        /// Word 'Cancel' and its translation on the settings page
        /// </summary>
        public Word CancelField { get; private set; } = new Word("Cancel"); 
        #endregion  // Buttons fields

        #region Editor methods
        /// <summary>
        /// Sets translation for the word 'Editor'
        /// </summary>
        public void TranslateEditorField(string value) => EditorField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Language'
        /// </summary>
        public void TranslateLanguageField(string value) => LanguageField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'AutoSave'
        /// </summary>
        public void TranslateAutoSaveField(string value) => AutoSaveField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'FontSize'
        /// </summary>
        public void TranslateFontSizeField(string value) => FontSizeField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'FontFamily'
        /// </summary>
        public void TranslateFontFamilyField(string value) => FontFamilyField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'TabSize'
        /// </summary>
        public void TranslateTabSizeField(string value) => TabSizeField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'WordWrap'
        /// </summary>
        public void TranslateWordWrapField(string value) => WordWrapField.SetTranslation(value); 
        #endregion  // Editor methods

        #region DB methods
        /// <summary>
        /// Sets translation for the word 'Db'
        /// </summary>
        public void TranslateDbField(string value) => DbField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'DefaultRdbms'
        /// </summary>
        public void TranslateDefaultRdbmsField(string value) => DefaultRdbmsField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'ActiveRdbms'
        /// </summary>
        public void TranslateActiveRdbmsField(string value) => ActiveRdbmsField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Database'
        /// </summary>
        public void TranslateDatabaseField(string value) => DatabaseField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Port'
        /// </summary>
        public void TranslatePortField(string value) => PortField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Schema'
        /// </summary>
        public void TranslateSchemaField(string value) => SchemaField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Username'
        /// </summary>
        public void TranslateUsernameField(string value) => UsernameField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Password'
        /// </summary>
        public void TranslatePasswordField(string value) => PasswordField.SetTranslation(value); 
        #endregion  // DB methods

        #region Common field
        /// <summary>
        /// Sets translation for the word 'Enabled'
        /// </summary>
        public void TranslateEnabledField(string value) => EnabledField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Disabled'
        /// </summary>
        public void TranslateDisabledField(string value) => DisabledField.SetTranslation(value); 
        #endregion  // Common field

        #region Common field
        /// <summary>
        /// Sets translation for the word 'ChosenLanguage'
        /// </summary>
        public void TranslateChosenLanguageField(string value) => ChosenLanguageField.SetTranslation(value); 
        #endregion  // Common field

        #region Buttons methods
        /// <summary>
        /// Sets translation for the word 'Recover'
        /// </summary>
        public void TranslateRecoverField(string value) => RecoverField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Save'
        /// </summary>
        public void TranslateSaveField(string value) => SaveField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Cancel'
        /// </summary>
        public void TranslateCancelField(string value) => CancelField.SetTranslation(value); 
        #endregion  // Buttons methods
    }
}
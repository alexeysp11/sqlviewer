using SqlViewer.Entities.Language;

namespace SqlViewer.Entities.ViewsEntities
{
    public class SettingsEntity 
    {
        #region Editor fields
        public Word EditorField { get; private set; } = new Word("Editor"); 
        public Word LanguageField { get; private set; } = new Word("Language"); 
        public Word AutoSaveField { get; private set; } = new Word("Auto save"); 
        public Word FontSizeField { get; private set; } = new Word("Font size"); 
        public Word FontFamilyField { get; private set; } = new Word("Font family"); 
        public Word TabSizeField { get; private set; } = new Word("Tab size"); 
        public Word WordWrapField { get; private set; } = new Word("Word wrap"); 
        #endregion  // Editor fields

        #region DB fields
        public Word DbField { get; private set; } = new Word("DB"); 
        public Word DefaultRdbmsField { get; private set; } = new Word("Default RDBMS"); 
        public Word ActiveRdbmsField { get; private set; } = new Word("Active RDBMS"); 
        public Word DatabaseField { get; private set; } = new Word("Database"); 
        public Word SchemaField { get; private set; } = new Word("Schema"); 
        public Word UsernameField { get; private set; } = new Word("Username"); 
        public Word PasswordField { get; private set; } = new Word("Password"); 
        #endregion  // DB fields

        #region Common fields
        public Word EnabledField { get; private set; } = new Word("Enabled"); 
        public Word DisabledField { get; private set; } = new Word("Disabled"); 
        #endregion  // Common fields

        #region Common fields
        public Word ChosenLanguageField { get; private set; } = new Word("Language"); 
        #endregion  // Common fields

        #region Buttons fields
        public Word RecoverField { get; private set; } = new Word("Recover"); 
        public Word SaveField { get; private set; } = new Word("Save"); 
        public Word CancelField { get; private set; } = new Word("Cancel"); 
        #endregion  // Buttons fields

        #region Editor methods
        public void SetEditorField(string value)
        {
            EditorField.SetTranslation(value); 
        }

        public void SetLanguageField(string value)
        {
            LanguageField.SetTranslation(value); 
        }

        public void SetAutoSaveField(string value)
        {
            AutoSaveField.SetTranslation(value); 
        }

        public void SetFontSizeField(string value)
        {
            FontSizeField.SetTranslation(value); 
        }

        public void SetFontFamilyField(string value)
        {
            FontFamilyField.SetTranslation(value); 
        }

        public void SetTabSizeField(string value)
        {
            TabSizeField.SetTranslation(value); 
        }

        public void SetWordWrapField(string value)
        {
            WordWrapField.SetTranslation(value); 
        }

        #endregion  // Editor methods

        #region DB methods
        public void SetDbField(string value)
        {
            DbField.SetTranslation(value); 
        }

        public void SetDefaultRdbmsField(string value)
        {
            DefaultRdbmsField.SetTranslation(value); 
        }

        public void SetActiveRdbmsField(string value)
        {
            ActiveRdbmsField.SetTranslation(value); 
        }

        public void SetDatabaseField(string value)
        {
            DatabaseField.SetTranslation(value); 
        }

        public void SetSchemaField(string value)
        {
            SchemaField.SetTranslation(value); 
        }

        public void SetUsernameField(string value)
        {
            UsernameField.SetTranslation(value); 
        }

        public void SetPasswordField(string value)
        {
            PasswordField.SetTranslation(value); 
        }
        #endregion  // DB methods

        #region Common field
        public void SetEnabledField(string value)
        {
            EnabledField.SetTranslation(value); 
        }

        public void SetDisabledField(string value)
        {
            DisabledField.SetTranslation(value); 
        }
        #endregion  // Common field

        #region Common field
        public void SetChosenLanguageField(string value)
        {
            ChosenLanguageField.SetTranslation(value); 
        }
        #endregion  // Common field

        #region Buttons methods
        public void SetRecoverField(string value)
        {
            RecoverField.SetTranslation(value); 
        }
        
        public void SetSaveField(string value)
        {
            SaveField.SetTranslation(value); 
        }
        
        public void SetCancelField(string value)
        {
            CancelField.SetTranslation(value); 
        }
        #endregion  // Buttons methods
    }
}
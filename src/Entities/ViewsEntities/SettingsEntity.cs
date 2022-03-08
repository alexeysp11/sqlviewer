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
        public void SetEditorField(System.String value)
        {
            EditorField.SetTranslation(value); 
        }

        public void SetLanguageField(System.String value)
        {
            LanguageField.SetTranslation(value); 
        }

        public void SetAutoSaveField(System.String value)
        {
            AutoSaveField.SetTranslation(value); 
        }

        public void SetFontSizeField(System.String value)
        {
            FontSizeField.SetTranslation(value); 
        }

        public void SetFontFamilyField(System.String value)
        {
            FontFamilyField.SetTranslation(value); 
        }

        public void SetTabSizeField(System.String value)
        {
            TabSizeField.SetTranslation(value); 
        }

        public void SetWordWrapField(System.String value)
        {
            WordWrapField.SetTranslation(value); 
        }

        #endregion  // Editor methods

        #region DB methods
        public void SetDbField(System.String value)
        {
            DbField.SetTranslation(value); 
        }

        public void SetDefaultRdbmsField(System.String value)
        {
            DefaultRdbmsField.SetTranslation(value); 
        }

        public void SetActiveRdbmsField(System.String value)
        {
            ActiveRdbmsField.SetTranslation(value); 
        }

        public void SetDatabaseField(System.String value)
        {
            DatabaseField.SetTranslation(value); 
        }

        public void SetSchemaField(System.String value)
        {
            SchemaField.SetTranslation(value); 
        }

        public void SetUsernameField(System.String value)
        {
            UsernameField.SetTranslation(value); 
        }

        public void SetPasswordField(System.String value)
        {
            PasswordField.SetTranslation(value); 
        }
        #endregion  // DB methods

        #region Common field
        public void SetEnabledField(System.String value)
        {
            EnabledField.SetTranslation(value); 
        }

        public void SetDisabledField(System.String value)
        {
            DisabledField.SetTranslation(value); 
        }
        #endregion  // Common field

        #region Common field
        public void SetChosenLanguageField(System.String value)
        {
            ChosenLanguageField.SetTranslation(value); 
        }
        #endregion  // Common field

        #region Buttons methods
        public void SetRecoverField(System.String value)
        {
            RecoverField.SetTranslation(value); 
        }
        
        public void SetSaveField(System.String value)
        {
            SaveField.SetTranslation(value); 
        }
        
        public void SetCancelField(System.String value)
        {
            CancelField.SetTranslation(value); 
        }
        #endregion  // Buttons methods
    }
}
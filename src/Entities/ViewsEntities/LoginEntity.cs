using SqlViewer.Entities.Language;

namespace SqlViewer.Entities.ViewsEntities
{
    public class LoginEntity
    {
        public Word ActiveRdbmsField { get; private set; } = new Word("Active RDBMS"); 
        public Word DatabaseField { get; private set; } = new Word("Database"); 
        public Word SchemaField { get; private set; } = new Word("Schema"); 
        public Word UsernameField { get; private set; } = new Word("Username"); 
        public Word PasswordField { get; private set; } = new Word("Password"); 

        public Word LogInField { get; private set; } = new Word("Log In"); 
        public Word CancelField { get; private set; } = new Word("Cancel"); 

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

        public void SetLogInField(System.String value)
        {
            LogInField.SetTranslation(value); 
        }

        public void SetCancelField(System.String value)
        {
            CancelField.SetTranslation(value); 
        }
    }
}
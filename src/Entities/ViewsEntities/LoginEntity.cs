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

        public void SetLogInField(string value)
        {
            LogInField.SetTranslation(value); 
        }

        public void SetCancelField(string value)
        {
            CancelField.SetTranslation(value); 
        }
    }
}
using SqlViewer.Entities.Language;

namespace SqlViewer.Entities.ViewsEntities
{
    /// <summary>
    /// Represents an entity of translating the login page 
    /// </summary>
    public class LoginEntity
    {
        /// <summary>
        /// Word 'Active RDBMS' and its translation on the login page
        /// </summary>
        public Word ActiveRdbmsField { get; private set; } = new Word("Active RDBMS"); 
        /// <summary>
        /// Word 'Database' and its translation on the login page
        /// </summary>
        public Word DatabaseField { get; private set; } = new Word("Database"); 
        /// <summary>
        /// Word 'Port' and its translation on the login page
        /// </summary>
        public Word PortField { get; private set; } = new Word("Port"); 
        /// <summary>
        /// Word 'Schema' and its translation on the login page
        /// </summary>
        public Word SchemaField { get; private set; } = new Word("Schema"); 
        /// <summary>
        /// Word 'Username' and its translation on the login page
        /// </summary>
        public Word UsernameField { get; private set; } = new Word("Username"); 
        /// <summary>
        /// Word 'Password' and its translation on the login page
        /// </summary>
        public Word PasswordField { get; private set; } = new Word("Password"); 

        /// <summary>
        /// Word 'LogIn' and its translation on the login page
        /// </summary>
        public Word LogInField { get; private set; } = new Word("Log In"); 
        /// <summary>
        /// Word 'Cancel' and its translation on the login page
        /// </summary>
        public Word CancelField { get; private set; } = new Word("Cancel"); 

        /// <summary>
        /// Sets translation for the word 'Active RDBMS'
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

        /// <summary>
        /// Sets translation for the word 'LogIn'
        /// </summary>
        public void TranslateLogInField(string value) => LogInField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Cancel'
        /// </summary>
        public void TranslateCancelField(string value) => CancelField.SetTranslation(value); 
    }
}
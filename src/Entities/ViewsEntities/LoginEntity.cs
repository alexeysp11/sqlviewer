using SqlViewer.Entities.Language;

namespace SqlViewer.Entities.ViewsEntities
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginEntity
    {
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

        /// <summary>
        /// 
        /// </summary>
        public Word LogInField { get; private set; } = new Word("Log In"); 
        /// <summary>
        /// 
        /// </summary>
        public Word CancelField { get; private set; } = new Word("Cancel"); 

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

        /// <summary>
        /// 
        /// </summary>
        public void TranslateLogInField(string value) => LogInField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateCancelField(string value) => CancelField.SetTranslation(value); 
    }
}
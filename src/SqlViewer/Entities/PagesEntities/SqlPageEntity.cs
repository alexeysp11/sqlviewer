using SqlViewer.Entities.Language;

namespace SqlViewer.Entities.PagesEntities
{
    /// <summary>
    /// Represents an entity of translating the SQL page 
    /// </summary>
    public class SqlPageEntity 
    {
        /// <summary>
        /// Word 'Path' and its translation on the SQL page
        /// </summary>
        public Word PathField { get; private set; } = new Word("Path"); 
        /// <summary>
        /// Word 'Execute' and its translation on the SQL page
        /// </summary>
        public Word ExecuteField { get; private set; } = new Word("Execute"); 

        /// <summary>
        /// Sets translation for the word 'Path'
        /// </summary>
        public void TranslatePathField(string value) => PathField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Execute'
        /// </summary>
        public void TranslateExecuteField(string value) => ExecuteField.SetTranslation(value); 
    }
}
using SqlViewer.Entities.Language;

namespace SqlViewer.Entities.UserControlsEntities
{
    /// <summary>
    /// Represents an entity of translating the connections page 
    /// </summary>
    public class ConnectionEntity
    {
        /// <summary>
        /// Word 'Active RDBMS' and its translation on the connections page
        /// </summary>
        public Word ActiveRdbmsField { get; private set; } = new Word("Active RDBMS"); 
        /// <summary>
        /// Word 'Execute' and its translation on the connections page
        /// </summary>
        public Word ExecuteField { get; private set; } = new Word("Execute"); 
        /// <summary>
        /// Word 'Transfer' and its translation on the connections page
        /// </summary>
        public Word TransferField { get; private set; } = new Word("Transfer"); 

        /// <summary>
        /// Sets translation for the word 'Active RDBMS'
        /// </summary>
        public void TranslateActiveRdbmsField(string value) => ActiveRdbmsField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Execute'
        /// </summary>
        public void TranslateExecuteField(string value) => ExecuteField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Transfer'
        /// </summary>
        public void TranslateTransferField(string value) => TransferField.SetTranslation(value); 
    }
}
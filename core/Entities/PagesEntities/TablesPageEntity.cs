using SqlViewer.Entities.Language;

namespace SqlViewer.Entities.PagesEntities
{
    /// <summary>
    /// Represents an entity of translating the tables page 
    /// </summary>
    public class TablesPageEntity 
    {
        /// <summary>
        /// Word 'Path' and its translation on the tables page
        /// </summary>
        public Word PathField { get; private set; } = new Word("Path"); 
        /// <summary>
        /// Word 'Tables' and its translation on the tables page
        /// </summary>
        public Word TablesField { get; private set; } = new Word("Tables"); 
        /// <summary>
        /// Word 'General info' and its translation on the tables page
        /// </summary>
        public Word GeneralInfoField { get; private set; } = new Word("General info"); 
        /// <summary>
        /// Word 'Columns' and its translation on the tables page
        /// </summary>
        public Word ColumnsField { get; private set; } = new Word("Columns"); 
        /// <summary>
        /// Word 'Foreign keys' and its translation on the tables page
        /// </summary>
        public Word ForeignKeysField { get; private set; } = new Word("Foreign keys"); 
        /// <summary>
        /// Word 'Triggers' and its translation on the tables page
        /// </summary>
        public Word TriggersField { get; private set; } = new Word("Triggers"); 
        /// <summary>
        /// Word 'Data' and its translation on the tables page
        /// </summary>
        public Word DataField { get; private set; } = new Word("Data"); 

        /// <summary>
        /// Sets translation for the word 'Path'
        /// </summary>
        public void TranslatePathField(string value) => PathField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Tables'
        /// </summary>
        public void TranslateTablesField(string value) => TablesField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'General info'
        /// </summary>
        public void TranslateGeneralInfoField(string value) => GeneralInfoField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Columns'
        /// </summary>
        public void TranslateColumnsField(string value) => ColumnsField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Foreign keys'
        /// </summary>
        public void TranslateForeignKeysField(string value) => ForeignKeysField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Triggers'
        /// </summary>
        public void TranslateTriggersField(string value) => TriggersField.SetTranslation(value); 
        /// <summary>
        /// Sets translation for the word 'Data'
        /// </summary>
        public void TranslateDataField(string value) => DataField.SetTranslation(value); 
    }
}
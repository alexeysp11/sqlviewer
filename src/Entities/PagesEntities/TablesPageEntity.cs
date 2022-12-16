using SqlViewer.Entities.Language;

namespace SqlViewer.Entities.PagesEntities
{
    public class TablesPageEntity 
    {
        public Word PathField { get; private set; } = new Word("Path"); 
        public Word TablesField { get; private set; } = new Word("Tables"); 
        public Word GeneralInfoField { get; private set; } = new Word("General info"); 
        public Word ColumnsField { get; private set; } = new Word("Columns"); 
        public Word ForeignKeysField { get; private set; } = new Word("Foreign keys"); 
        public Word TriggersField { get; private set; } = new Word("Triggers"); 
        public Word DataField { get; private set; } = new Word("Data"); 

        public void SetPathField(string value) => PathField.SetTranslation(value); 
        public void SetTablesField(string value) => TablesField.SetTranslation(value); 
        public void SetGeneralInfoField(string value) => GeneralInfoField.SetTranslation(value); 
        public void SetColumnsField(string value) => ColumnsField.SetTranslation(value); 
        public void SetForeignKeysField(string value) => ForeignKeysField.SetTranslation(value); 
        public void SetTriggersField(string value) => TriggersField.SetTranslation(value); 
        public void SetDataField(string value) => DataField.SetTranslation(value); 
    }
}
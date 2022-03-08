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

        public void SetPathField(System.String value)
        {
            PathField.SetTranslation(value); 
        }

        public void SetTablesField(System.String value)
        {
            TablesField.SetTranslation(value); 
        }

        public void SetGeneralInfoField(System.String value)
        {
            GeneralInfoField.SetTranslation(value); 
        }

        public void SetColumnsField(System.String value)
        {
            ColumnsField.SetTranslation(value); 
        }

        public void SetForeignKeysField(System.String value)
        {
            ForeignKeysField.SetTranslation(value); 
        }

        public void SetTriggersField(System.String value)
        {
            TriggersField.SetTranslation(value); 
        }

        public void SetDataField(System.String value)
        {
            DataField.SetTranslation(value); 
        }
    }
}
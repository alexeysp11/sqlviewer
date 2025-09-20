using SqlViewer.Entities.Language;

namespace SqlViewer.Entities.PagesEntities
{
    public class SqlPageEntity 
    {
        public Word PathField { get; private set; } = new Word("Path"); 
        public Word ExecuteField { get; private set; } = new Word("Execute"); 

        public void SetPathField(string value) => PathField.SetTranslation(value); 
        public void SetExecuteField(string value) => ExecuteField.SetTranslation(value); 
    }
}
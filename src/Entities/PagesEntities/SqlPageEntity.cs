using SqlViewer.Entities.Language;

namespace SqlViewer.Entities.PagesEntities
{
    public class SqlPageEntity 
    {
        public Word PathField { get; private set; } = new Word("Path"); 
        public Word ExecuteField { get; private set; } = new Word("Execute"); 

        public void SetPathField(System.String value)
        {
            PathField.SetTranslation(value); 
        }

        public void SetExecuteField(System.String value)
        {
            ExecuteField.SetTranslation(value); 
        }
    }
}
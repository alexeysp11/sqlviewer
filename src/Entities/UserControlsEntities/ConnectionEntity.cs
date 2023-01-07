using SqlViewer.Entities.Language;

namespace SqlViewer.Entities.UserControlsEntities
{
    public class ConnectionEntity
    {
        public Word ActiveRdbmsField { get; private set; } = new Word("Active RDBMS"); 
        public Word ExecuteField { get; private set; } = new Word("Execute"); 

        public void SetActiveRdbmsField(string value) => ActiveRdbmsField.SetTranslation(value); 
        public void SetExecuteField(string value) => ExecuteField.SetTranslation(value); 
    }
}
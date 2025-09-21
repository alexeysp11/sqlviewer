using SqlViewer.Entities.Language;

namespace SqlViewer.Entities.UserControlsEntities
{
    public class ConnectionEntity
    {
        public Word ActiveRdbmsField { get; private set; } = new Word("Active RDBMS"); 
        public Word ExecuteField { get; private set; } = new Word("Execute"); 
        public Word TransferField { get; private set; } = new Word("Transfer"); 

        public void SetActiveRdbmsField(string value) => ActiveRdbmsField.SetTranslation(value); 
        public void SetExecuteField(string value) => ExecuteField.SetTranslation(value); 
        public void SetTransferField(string value) => TransferField.SetTranslation(value); 
    }
}
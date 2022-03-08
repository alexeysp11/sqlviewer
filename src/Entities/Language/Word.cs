namespace SqlViewer.Entities.Language
{
    public class Word 
    {
        public System.String English { get; private set; }
        public System.String Translation { get; private set; }
        
        public Word(System.String english)
        {
            English = english; 
        }
        
        public Word(System.String english, System.String translation)
        {
            English = english; 
            Translation = translation; 
        }

        public void SetTranslation(System.String translation)
        {
            Translation = translation; 
        }
    }
}
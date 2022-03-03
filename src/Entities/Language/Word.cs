namespace SqlViewer.Entities.Language
{
    public class Word 
    {
        public string English { get; private set; }
        public string Translation { get; private set; }
        
        public Word(string english)
        {
            English = english; 
        }
        
        public Word(string english, string translation)
        {
            English = english; 
            Translation = translation; 
        }

        public void SetTranslation(string translation)
        {
            Translation = translation; 
        }
    }
}
namespace SqlViewer.Entities.Language
{
    /// <summary>
    /// Represents a word or a phrase that is supposed to be translated 
    /// </summary>
    public class Word 
    {
        #region Properties
        /// <summary>
        /// Meaning of the word in English 
        /// </summary>
        public string English { get; private set; }
        /// <summary>
        /// Corresponding translation of the word 
        /// </summary>
        public string Translation { get; private set; }
        #endregion  // Properties
        
        #region Constructors
        /// <summary>
        /// Basic constructor that initializes only the meaning of a word in English, 
        /// so translation is supposed to be set using SetTranslation method
        /// </summary>
        public Word(string english)
        {
            English = english; 
        }
        
        /// <summary>
        /// Constructor that initializes both the meaning of a word in English and its translation 
        /// </summary>
        public Word(string english, string translation)
        {
            English = english; 
            Translation = translation; 
        }
        #endregion  // Constructors

        #region Public methods
        /// <summary>
        /// Sets translation of the word 
        /// </summary>
        public void SetTranslation(string translation)
        {
            Translation = translation; 
        }
        #endregion  // Public methods
    }
}
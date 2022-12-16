using SqlViewer.Models.EnumOperations; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Helpers 
{
    public static class EnumCodecHelper
    {
        public static EnumEncoder EnumEncoder {get; private set; } = new EnumEncoder(); 
        public static EnumDecoder EnumDecoder {get; private set; } = new EnumDecoder(); 
    }
}
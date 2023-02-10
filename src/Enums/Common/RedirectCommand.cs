namespace SqlViewer.Enums.Common
{
    /// <summary>
    /// Enumeration for stroing possible parameters that could be passed for performing RedirectCommand functionality 
    /// </summary>
    public enum RedirectCommand
    {
        SqlQuery = 1,
        Tables = 2,
        Settings = 3,
        Connections = 4,
        About = 5,
        SqliteDocs = 6,
        PosgresDocs = 7,
        MySqlDocs = 8,
        OracleDocs = 9
    }
}

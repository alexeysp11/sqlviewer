using System.Data;
using System.Linq;
using SqlViewer.Models.DbConnections;

namespace SqlViewer.Utils.Language;

public abstract class BaseTranslator
{
    private SqliteDbConnection AppDbConnection { get; set; }

    public void SetAppDbConnection(SqliteDbConnection appDbConnection)
    {
        AppDbConnection = appDbConnection;
    }

    public DataTable Translate(string sql) => AppDbConnection.ExecuteSqlCommand(sql);

    public static string TranslateSingleWord(DataTable dt, string rowName, string wordEnglish)
    {
        if (dt is null || !dt.Columns.Contains(rowName))
        {
            return string.Empty;
        }
        DataRow row = dt
            .AsEnumerable()
            .Where(row => row.Field<string>("english") == wordEnglish)
            .FirstOrDefault();
        return row is not null ? row[rowName].ToString() : string.Empty;
    }
}
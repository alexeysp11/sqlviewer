using System.Data;

namespace SqlViewer.Helpers;

public abstract class TranslateHelper
{
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
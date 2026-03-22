namespace SqlViewer.Shared.Helpers.DataTransfer;

public static class DataTransferCursorHelper
{
    public static string EncodeCursor(DateTime createdAt, long id)
    {
        // 8 bytes for Ticks, 8 for Id
        byte[] bytes = new byte[16]; 

        BitConverter.TryWriteBytes(bytes.AsSpan(0, 8), createdAt.Ticks);
        BitConverter.TryWriteBytes(bytes.AsSpan(8, 8), id);

        return Convert.ToBase64String(bytes);
    }

    public static (DateTime CreatedAt, long Id)? DecodeCursor(string? cursor)
    {
        if (string.IsNullOrEmpty(cursor)) return null;
        try
        {
            byte[] bytes = Convert.FromBase64String(cursor);
            long ticks = BitConverter.ToInt64(bytes, 0);
            long id = BitConverter.ToInt64(bytes, 8);
            return (new DateTime(ticks, DateTimeKind.Utc), id);
        }
        catch { return null; }
    }
}

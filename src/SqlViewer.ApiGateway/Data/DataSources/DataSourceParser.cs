namespace SqlViewer.ApiGateway.Data.DataSources;

using System.Text.RegularExpressions;

public static partial class DataSourceParser
{
    /// <summary>
    /// Regular expression to find <c>Id</c> and <c>Name</c> inside <c>[DataSource ...]</c> string.
    /// </summary>
    private static readonly Regex DataSourceRegex = DataSourceParseRegex();

    public static DataSourceParseInfo? Parse(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return null;
        
        Match match = DataSourceRegex.Match(input);
        if (!match.Success)
            return null;

        bool isIntId = int.TryParse(match.Groups["id"].Value, out int id);
        string? name = match.Groups["name"].Value;

        return new DataSourceParseInfo
        {
            Id = isIntId ? id : null,
            Name = string.IsNullOrEmpty(name) ? null : name
        };
    }

    [GeneratedRegex(pattern: @"\[DataSource\s+(?:Id=""(?<id>[^""]+)""|Name=""(?<name>[^""]+)""|\s+)+\]", options: RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex DataSourceParseRegex();
}

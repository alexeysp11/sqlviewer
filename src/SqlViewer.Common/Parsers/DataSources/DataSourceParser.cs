namespace SqlViewer.ApiGateway.Data.DataSources;

using System.Text.RegularExpressions;
using SqlViewer.Common.Parsers.DataSources;

public static partial class DataSourceParser
{
    /// <summary>
    /// Regular expression to find <c>Id</c> and <c>Name</c> inside <c>[DataSource ...]</c> string.
    /// </summary>
    private static readonly Regex DataSourceRegex = DataSourceParseRegex();

    private const string IdRegexGroupName = "id";
    private const string NameRegexGroupName = "name";

    public static DataSourceParseInfo? Parse(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return null;
        
        Match match = DataSourceRegex.Match(input);
        if (!match.Success)
            return null;

        string? dataSourceName = match.Groups[NameRegexGroupName].Value;
        return new DataSourceParseInfo
        {
            Id = int.TryParse(match.Groups[IdRegexGroupName].Value, out int dataSourceId) ? dataSourceId : null,
            Name = string.IsNullOrEmpty(dataSourceName) ? null : dataSourceName
        };
    }

    [GeneratedRegex(pattern: $@"\[DataSource\s+(?:Id=""(?<{IdRegexGroupName}>[^""]+)""|Name=""(?<{NameRegexGroupName}>[^""]+)""|\s+)+\]", options: RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex DataSourceParseRegex();
}

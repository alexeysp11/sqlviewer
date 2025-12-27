using Newtonsoft.Json;
using SqlViewer.Common.Dtos.SqlQueries;
using SqlViewer.Common.Enums;
using System.Data;
using System.Net.Http;
using System.Net.Http.Json;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace SqlViewer.Services;

public sealed class SqlApiService : ISqlApiService, IDisposable
{
    private readonly HttpClient _httpClient;

    private const string QueryPath = "api/sql/query";

    public SqlApiService()
    {
        _httpClient = new()
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    public async Task<DataTable> QueryAsync(VelocipedeDatabaseType databaseType, string connectionString, string query)
    {
        SqlQueryRequestDto requestDto = new()
        {
            DatabaseType = databaseType,
            ConnectionString = connectionString,
            Query = query
        };

        UriBuilder uriBuilder = new()
        {
            Scheme = App.AppSettings.ServerScheme,
            Host = App.AppSettings.ServerHost,
            Port = App.AppSettings.ServerPort,
            Path = QueryPath,
        };
        string url = uriBuilder.Uri.ToString();

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, requestDto);
        response.EnsureSuccessStatusCode();

        string jsonResponse = await response.Content.ReadAsStringAsync();

        SqlQueryResponseDto responseDto = JsonConvert.DeserializeObject<SqlQueryResponseDto>(jsonResponse);
        if (responseDto is null || responseDto.Status is SqlOperationStatus.None)
            throw new InvalidOperationException("Unable to get response DTO");
        if (responseDto.Status is SqlOperationStatus.Failed)
            throw new InvalidOperationException(responseDto.ErrorMessage);

        return responseDto.QueryResult.ToDataTable();
    }
}

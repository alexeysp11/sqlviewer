using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Dtos;
using System.Text.Json;
using System.Text;
using SqlViewer.Shared.Dtos.Etl;
using SqlViewer.Shared.Converters;
using SqlViewer.StorageContexts;

namespace SqlViewer.ApiHandlers.Implementations;

public sealed class EtlHttpHandler : HttpHandler, IEtlHttpHandler
{
    private readonly IUserContext _userContext;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public EtlHttpHandler(IUserContext userContext) : base()
    {
        _userContext = userContext;
        _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new DynamicObjectConverter() }
        };
    }

    public async Task<StartTransferResponseDto> PostStartTransferAsync(StartTransferRequestDto requestDto)
    {
        UriBuilder uriBuilder = new()
        {
            Scheme = App.AppSettings.ServerScheme,
            Host = App.AppSettings.ServerHost,
            Port = App.AppSettings.ServerPort,
            Path = RestApiPaths.Etl.DataTransfer.Start,
        };
        string url = uriBuilder.Uri.ToString();

        // Authorization.
        using HttpRequestMessage requestMessage = new(HttpMethod.Post, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(_userContext.TokenType, _userContext.AccessToken);
        requestMessage.Content = new StringContent(JsonSerializer.Serialize(requestDto), Encoding.UTF8, "application/json");

        // Request.
        HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage;
            try
            {
                ProblemDetailsResponseDto problem = JsonSerializer.Deserialize<ProblemDetailsResponseDto>(jsonResponse, _jsonSerializerOptions);
                errorMessage = problem?.Detail ?? problem?.Title ?? jsonResponse;
            }
            catch
            {
                errorMessage = string.IsNullOrEmpty(jsonResponse) ? $"Status code: {response.StatusCode}" : jsonResponse;
            }
            throw new Exception(errorMessage);
        }
        StartTransferResponseDto responseDto = JsonSerializer.Deserialize<StartTransferResponseDto>(jsonResponse, _jsonSerializerOptions);
        return responseDto;
    }

    public async Task<TransferStatusResponseDto> GetTransferStatusAsync(Guid correlationId)
    {
        // Sending a GET request to retrieve the status of a specific saga
        return await _httpClient.GetFromJsonAsync<TransferStatusResponseDto>($"api/etl/status/{correlationId}")
           ?? throw new InvalidOperationException($"Failed to deserialize {nameof(TransferStatusResponseDto)}");
    }
}

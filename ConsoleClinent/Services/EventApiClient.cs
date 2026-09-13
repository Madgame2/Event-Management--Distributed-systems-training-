using System.Net.Http.Json;
using Server.Application.DTO;

namespace ConsoleClinet.Services;

public class EventApiClient : IEventApiClient
{
    private readonly HttpClient _httpClient;
    
    public EventApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<string> CreateEventAsync(CreateEventCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync("api/events", command);
        return await ProcessResponseAsync(response);
    }

    public async Task<string> RegisterParticipantAsync(RegisterParticipantCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync("api/events/register", command);
        return await ProcessResponseAsync(response);
    }
    
    private static async Task<string> ProcessResponseAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = string.IsNullOrWhiteSpace(content) 
                ? "Сервер не вернул деталей ошибки." 
                : content;
                
            throw new HttpRequestException($"[{(int)response.StatusCode} {response.StatusCode}] {errorMessage}");
        }

        return string.IsNullOrWhiteSpace(content) ? "Операция успешно выполнена." : content;
    }
}
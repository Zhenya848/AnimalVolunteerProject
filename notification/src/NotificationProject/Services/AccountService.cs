using System.Text;
using NotificationProject.Services.Requests;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationProject.Entities;
using NotificationProject.Entities.User;

namespace NotificationProject.Services;

public class AccountService
{
    private readonly HttpClient _httpClient;

    public AccountService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }
    
    public async Task<IEnumerable<UserDto>> GetUsers(
        GetUsersRequest request,
        CancellationToken cancellationToken = default)
    {
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await _httpClient
            .PostAsync(Constants.API_URL, content, cancellationToken);
        
        if (response.IsSuccessStatusCode)
        {
            var jsonSerializerOptions = new JsonSerializerOptions()
            { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
            var users = JsonSerializer.Deserialize<UserDto[]>(jsonResponse, jsonSerializerOptions);
            
            return users ?? [];
        }

        throw new Exception($"Ошибка вызова эндпоинта: {response.StatusCode}");
    }
}
using System.Net.Http.Json;
using EMSFrontend.Frontend.Models;

namespace EMSFrontend.Frontend.Clients;

public class DepartmentsClient
{
    private readonly HttpClient _http;

    public DepartmentsClient(HttpClient httpClient) => _http = httpClient;

    public async Task<Department[]> GetDepartmentsAsync()
        => await _http.GetFromJsonAsync<Department[]>("/departments") ?? Array.Empty<Department>();
}

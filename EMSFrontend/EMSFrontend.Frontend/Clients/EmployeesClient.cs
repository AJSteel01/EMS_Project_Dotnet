using System.Text.Json;
using EMSFrontend.Frontend.Models;

namespace EMSFrontend.Frontend.Clients;

public class EmployeesClient
{
    private readonly HttpClient _http;

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public EmployeesClient(HttpClient httpClient) => _http = httpClient;

    // NEW Search + Pagination Endpoint
    public async Task<PagedEmployeesResponse> GetEmployeesPagedAsync(string? search, int page, int pageSize)
    {
        string url = $"/employees?search={search}&page={page}&pageSize={pageSize}";
        var response = await _http.GetFromJsonAsync<PagedEmployeesResponse>(url, _options);

        return response ?? new PagedEmployeesResponse();
    }


    // Old Endpoint 
    public async Task<EmployeeSummary[]> GetEmployeesAsync()
        => await _http.GetFromJsonAsync<EmployeeSummary[]>("/employees", _options)
           ?? Array.Empty<EmployeeSummary>();

    public async Task<EmployeeDetails> GetEmployeeAsync(int id)
{
    var emp = await _http.GetFromJsonAsync<EmployeeDetails>($"/employees/{id}", _options)
       ?? throw new Exception("Employee not found");

    return emp;
}


    public async Task AddEmployeeAsync(EmployeeDetails employee)
        => await _http.PostAsJsonAsync("/employees", employee, _options);

    public async Task UpdateEmployeeAsync(EmployeeDetails employee)
        => await _http.PutAsJsonAsync($"/employees/{employee.EmpId}", employee, _options);

    public async Task DeleteEmployeeAsync(int id)
        => await _http.DeleteAsync($"/employees/{id}");

    public async Task UpdateSelfAsync(SelfUpdateRequest dto)
    {
        await _http.PutAsJsonAsync("/employees/self", dto);
    }

    public async Task UpdateDepartmentAsync(int empId, int departmentId)
{
    var body = new { DepartmentId = departmentId };

    await _http.PutAsJsonAsync($"/employees/{empId}/department", body);
}




}

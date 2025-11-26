using System.Net.Http.Json;

namespace EMSFrontend.Frontend.Services;

public class UserContext
{
    private readonly HttpClient _http;

    public bool IsLoggedIn { get; private set; }
    public bool IsAdmin { get; private set; }
    public bool IsEmployee { get; private set; }

    public string? FullName { get; private set; }
    public string? Email { get; private set; }
    public int? EmployeeId { get; private set; }

    public UserContext(HttpClient http)
    {
        _http = http;
    }

    public async Task LoadAsync()
    {
        try
        {
            // NO LEADING SLASH — VERY IMPORTANT
            var result = await _http.GetFromJsonAsync<UserInfo>("auth/me");

            if (result == null)
            {
                Reset();
                return;
            }

            IsLoggedIn = true;
            FullName = result.FullName;
            Email = result.Email;
            EmployeeId = result.EmployeeId;

            IsAdmin = result.Roles.Contains("Admin");
            IsEmployee = result.Roles.Contains("Employee");
        }
        catch
        {
            Reset();
        }
    }

    private void Reset()
    {
        IsLoggedIn = false;
        IsAdmin = false;
        IsEmployee = false;
        FullName = null;
        Email = null;
        EmployeeId = null;
    }

    private class UserInfo
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public int? EmployeeId { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}

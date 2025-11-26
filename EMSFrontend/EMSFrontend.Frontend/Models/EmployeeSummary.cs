using System.Text.Json.Serialization;

namespace EMSFrontend.Frontend.Models;

public class EmployeeSummary
{
    [JsonPropertyName("empId")]
    public int EmpId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("department")]
    public string Department { get; set; } = string.Empty;

    [JsonPropertyName("salary")]
    public decimal Salary { get; set; }
}

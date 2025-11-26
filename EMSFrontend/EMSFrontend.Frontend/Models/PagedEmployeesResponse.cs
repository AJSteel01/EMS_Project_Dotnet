namespace EMSFrontend.Frontend.Models;

public class PagedEmployeesResponse
{
    public List<EmployeeSummary> Items { get; set; } = new();
    public int TotalCount { get; set; }
}

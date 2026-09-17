using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HyS.ControlTrabajos.Data;

internal sealed class SupabaseJobRepository
{
    private readonly HttpClient http = new() { BaseAddress = new Uri(SupabaseConfig.ProjectUrl) };
    private readonly AuthSession session;

    public SupabaseJobRepository(AuthSession session)
    {
        this.session = session;
        http.DefaultRequestHeaders.Add("apikey", SupabaseConfig.PublishableKey);
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session.AccessToken);
    }

    public async Task<List<Job>> GetAllAsync()
    {
        var url = $"/rest/v1/jobs?select=id,work_date,area,work,amount,status&user_id=eq.{session.User.Id}&order=work_date.desc,created_at.desc";
        var rows = await http.GetFromJsonAsync<List<JobRow>>(url) ?? new();
        return rows.Select(ToJob).ToList();
    }

    public async Task<Job> AddAsync(Job job)
    {
        var row = new JobWriteRow
        {
            UserId = session.User.Id,
            WorkDate = ParseDate(job.Date),
            Area = job.Area,
            Work = job.Work,
            Amount = job.Amount,
            Status = job.Status,
            Source = "windows"
        };
        using var request = new HttpRequestMessage(HttpMethod.Post, "/rest/v1/jobs") { Content = JsonContent.Create(row) };
        request.Headers.Add("Prefer", "return=representation");
        using var response = await http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<List<JobRow>>() ?? new();
        return created.Count > 0 ? ToJob(created[0]) : job;
    }

    private static DateOnly ParseDate(string value)
    {
        if (DateOnly.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result)) return result;
        return DateOnly.FromDateTime(DateTime.Today);
    }

    private static Job ToJob(JobRow row) => new()
    {
        Id = row.Id,
        Date = row.WorkDate.ToString("dd/MM/yyyy"),
        Area = row.Area,
        Work = row.Work,
        Amount = row.Amount,
        Status = row.Status
    };

    private sealed class JobRow
    {
        [JsonPropertyName("id")] public Guid Id { get; set; }
        [JsonPropertyName("work_date")] public DateOnly WorkDate { get; set; }
        [JsonPropertyName("area")] public string Area { get; set; } = "";
        [JsonPropertyName("work")] public string Work { get; set; } = "";
        [JsonPropertyName("amount")] public decimal Amount { get; set; }
        [JsonPropertyName("status")] public string Status { get; set; } = "Pendiente";
    }

    private sealed class JobWriteRow
    {
        [JsonPropertyName("user_id")] public Guid UserId { get; set; }
        [JsonPropertyName("work_date")] public DateOnly WorkDate { get; set; }
        [JsonPropertyName("area")] public string Area { get; set; } = "";
        [JsonPropertyName("work")] public string Work { get; set; } = "";
        [JsonPropertyName("amount")] public decimal Amount { get; set; }
        [JsonPropertyName("status")] public string Status { get; set; } = "Pendiente";
        [JsonPropertyName("source")] public string Source { get; set; } = "windows";
    }
}
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace HyS.ControlTrabajos.Data;

internal sealed class SupabaseAuthService
{
    private readonly HttpClient http = new() { BaseAddress = new Uri(SupabaseConfig.ProjectUrl) };

    public async Task<AuthSession> SignInAsync(string email, string password)
    {
        if (!SupabaseConfig.IsConfigured)
            throw new InvalidOperationException("Falta configurar HYS_SUPABASE_PUBLISHABLE_KEY.");

        using var request = new HttpRequestMessage(HttpMethod.Post, "/auth/v1/token?grant_type=password")
        {
            Content = JsonContent.Create(new { email, password })
        };
        request.Headers.Add("apikey", SupabaseConfig.PublishableKey);
        using var response = await http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AuthSession>()
               ?? throw new InvalidOperationException("Supabase no devolvió una sesión válida.");
    }
}

internal sealed class AuthSession
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; } = "";
    [JsonPropertyName("refresh_token")] public string RefreshToken { get; set; } = "";
    [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }
    [JsonPropertyName("user")] public AuthUser User { get; set; } = new();
}

internal sealed class AuthUser
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; } = "";
}
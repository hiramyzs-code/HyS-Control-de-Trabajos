namespace HyS.ControlTrabajos.Data;

internal static class SupabaseConfig
{
    public const string ProjectUrl = "https://oxrwkinwlefzusmfjacw.supabase.co";

    // The publishable key is supplied at runtime/configuration time.
    // Never place a Supabase secret/service-role key in this desktop client.
    public static string PublishableKey =>
        Environment.GetEnvironmentVariable("HYS_SUPABASE_PUBLISHABLE_KEY") ?? string.Empty;

    public static bool IsConfigured => !string.IsNullOrWhiteSpace(PublishableKey);
}
namespace YatirimKoc.Application.Interfaces.Settings;

public interface ISiteSettingService
{
    Task<string?> GetValueAsync(string key);
    Task<IDictionary<string, string>> GetAllAsync();
    Task SetAsync(string key, string value, string? description = null);
}

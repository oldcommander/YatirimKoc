using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using YatirimKoc.Application.Interfaces.Settings;
using YatirimKoc.Domain.Entities.Settings;
using YatirimKoc.Infrastructure.Persistence;

namespace YatirimKoc.Infrastructure.Services.Settings;

public class SiteSettingService : ISiteSettingService
{
    private const string CacheKey = "SITE_SETTINGS";

    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _cache;

    public SiteSettingService(
        ApplicationDbContext context,
        IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<string?> GetValueAsync(string key)
    {
        var settings = await GetAllAsync();
        return settings.TryGetValue(key, out var value) ? value : null;
    }

    public async Task<IDictionary<string, string>> GetAllAsync()
    {
        if (_cache.TryGetValue(CacheKey, out Dictionary<string, string>? cached))
            return cached!;

        var data = await _context.SiteSettings
            .Where(x => x.IsActive)
            .ToDictionaryAsync(x => x.Key, x => x.Value);

        _cache.Set(CacheKey, data, TimeSpan.FromMinutes(30));

        return data;
    }

    public async Task SetAsync(string key, string value, string? description = null)
    {
        var entity = await _context.SiteSettings
            .FirstOrDefaultAsync(x => x.Key == key);

        if (entity == null)
        {
            entity = new SiteSetting
            {
                Key = key,
                Value = value,
                Description = description
            };
            _context.SiteSettings.Add(entity);
        }
        else
        {
            entity.Value = value;
            entity.Description = description;
        }

        await _context.SaveChangesAsync();
        _cache.Remove(CacheKey);
    }
}

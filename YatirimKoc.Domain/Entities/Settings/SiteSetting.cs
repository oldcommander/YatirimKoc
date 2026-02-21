using YatirimKoc.Domain.Common;

namespace YatirimKoc.Domain.Entities.Settings;

public class SiteSetting
{
    public Guid Id { get; set; }

    public string Key { get; set; } = default!;
    public string Value { get; set; } = default!;

    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime UpdatedAt { get; set; }
}

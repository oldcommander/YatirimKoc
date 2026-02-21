using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using YatirimKoc.Domain.Entities.Logs;
using YatirimKoc.Domain.Entities.Identity;

namespace YatirimKoc.Infrastructure.Persistence.Interceptors;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditSaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null) return result;

        var auditLogs = new List<AuditLog>();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog ||
                entry.State == EntityState.Detached ||
                entry.State == EntityState.Unchanged)
                continue;

            var auditLog = BuildAuditLog(entry);
            if (auditLog != null)
                auditLogs.Add(auditLog);
        }

        if (auditLogs.Any())
            await context.Set<AuditLog>().AddRangeAsync(auditLogs, cancellationToken);

        return result;
    }

    private AuditLog? BuildAuditLog(EntityEntry entry)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var userId = httpContext?.User?
            .FindFirst("sub")?.Value;

        var email = httpContext?.User?
            .Identity?.Name;

        var ip = httpContext?.Connection?.RemoteIpAddress?.ToString();

        return new AuditLog
        {
            TableName = entry.Metadata.GetTableName()!,
            Action = entry.State.ToString(),
            OldValues = entry.State == EntityState.Modified
                ? JsonSerializer.Serialize(entry.OriginalValues.ToObject())
                : null,
            NewValues = entry.State != EntityState.Deleted
                ? JsonSerializer.Serialize(entry.CurrentValues.ToObject())
                : null,
            UserId = Guid.TryParse(userId, out var id) ? id : null,
            UserEmail = email,
            IpAddress = ip
        };
    }
}

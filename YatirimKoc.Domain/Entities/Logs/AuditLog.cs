using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YatirimKoc.Domain.Common;

namespace YatirimKoc.Domain.Entities.Logs;

public class AuditLog
{
    public Guid Id { get; set; }

    public string TableName { get; set; } = default!;
    public string Action { get; set; } = default!; // Insert / Update / Delete

    public string? OldValues { get; set; }
    public string? NewValues { get; set; }

    public Guid? UserId { get; set; }
    public string? UserEmail { get; set; }
    public string? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}


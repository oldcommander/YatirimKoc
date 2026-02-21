using YatirimKoc.Domain.Enums;

namespace YatirimKoc.Domain.Entities.Messages;

public class ContactMessage
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }

    public string Subject { get; set; } = null!;
    public string Message { get; set; } = null!;

    public ContactMessageStatus Status { get; set; }
        = ContactMessageStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

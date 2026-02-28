using YatirimKoc.Domain.Enums;

namespace YatirimKoc.Application.Features.ContactMessages.Dtos;

public class ContactMessageDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string Subject { get; set; } = null!;
    public string Message { get; set; } = null!;
    public ContactMessageStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
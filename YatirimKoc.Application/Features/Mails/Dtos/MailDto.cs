namespace YatirimKoc.Application.Features.Mails.Dtos;

public class MailDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
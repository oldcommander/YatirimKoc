namespace YatirimKoc.Application.Features.MailTemplates.Dtos;

public class MailTemplateDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string BodyHtml { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
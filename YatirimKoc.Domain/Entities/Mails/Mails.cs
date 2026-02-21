namespace YatirimKoc.Domain.Entities.Mails;

public class MailTemplate
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;
    // OTP, ReservationApproved, PriceDrop, BlogAnnouncement

    public string Subject { get; set; } = null!;
    public string BodyHtml { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

using System;

namespace YatirimKoc.Application.Features.NewsletterSubscribers.Dtos
{
    public class MailLogDto
    {
        public Guid Id { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string TemplateCode { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
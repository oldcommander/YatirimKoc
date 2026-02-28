using System;

namespace YatirimKoc.Application.Features.NewsletterSubscribers.Dtos
{
    public class SubscriberDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
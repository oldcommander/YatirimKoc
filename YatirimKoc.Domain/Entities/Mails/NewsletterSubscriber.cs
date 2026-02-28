using System;
using YatirimKoc.Domain.Common;

namespace YatirimKoc.Domain.Entities.Mails
{
    public class NewsletterSubscriber : AuditableEntity
    {
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
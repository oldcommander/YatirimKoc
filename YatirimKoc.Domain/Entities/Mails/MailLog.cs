using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YatirimKoc.Domain.Entities.Mails
{
    public class MailLog
    {
        public Guid Id { get; set; }

        public string ToEmail { get; set; } = null!;
        public string Subject { get; set; } = null!;

        public string TemplateCode { get; set; } = null!;
        public bool IsSuccess { get; set; }

        public string? ErrorMessage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

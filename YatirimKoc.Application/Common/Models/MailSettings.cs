namespace YatirimKoc.Application.Common.Models
{
    public class MailSettings
    {
        public string FromEmail { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public bool UseSSL { get; set; }
    }
}
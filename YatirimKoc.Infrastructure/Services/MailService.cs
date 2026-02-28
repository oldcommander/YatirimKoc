using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Common.Models;
using YatirimKoc.Application.Interfaces;
using YatirimKoc.Domain.Entities.Mails;

namespace YatirimKoc.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IApplicationDbContext _context;

        // Ayarları ve Veritabanı context'ini Injection ile alıyoruz
        public MailService(IOptions<MailSettings> mailSettings, IApplicationDbContext context)
        {
            _mailSettings = mailSettings.Value;
            _context = context;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string templateCode, Dictionary<string, string> placeholders)
        {
            // 1. Şablonu Veritabanından (Mails tablosu) çek
            var template = await _context.Mails.FirstOrDefaultAsync(x => x.Code == templateCode && x.IsActive);

            if (template == null)
                return false; // Şablon bulunamadıysa veya aktif değilse işlemi iptal et

            // 2. Değişkenleri (Placeholder) HTML ve Başlık içine yerleştir
            string subject = ProcessTemplate(template.Subject, placeholders);
            string body = ProcessTemplate(template.BodyHtml, placeholders);

            // 3. Gönder ve Logla
            return await SendAndLogAsync(new List<string> { toEmail }, null, subject, body, templateCode);
        }

        public async Task<bool> SendBulkEmailAsync(List<string> bccEmails, string templateCode, Dictionary<string, string> placeholders)
        {
            var template = await _context.Mails.FirstOrDefaultAsync(x => x.Code == templateCode && x.IsActive);
            if (template == null) return false;

            string subject = ProcessTemplate(template.Subject, placeholders);
            string body = ProcessTemplate(template.BodyHtml, placeholders);

            // Toplu gönderimde kişileri BCC (Gizli Karbon Kopya) kısmına ekliyoruz ki kimse diğerinin mailini görmesin
            return await SendAndLogAsync(null, bccEmails, subject, body, templateCode);
        }

        // {{Degisken}} taglarını gerçek verilerle değiştiren metodumuz
        private string ProcessTemplate(string text, Dictionary<string, string> placeholders)
        {
            if (string.IsNullOrWhiteSpace(text) || placeholders == null) return text;

            foreach (var item in placeholders)
            {
                // C#'ta string interpolation içinde süslü parantez göstermek için çift süslü kullanılır.
                // ${{{{{item.Key}}}}} => Çıktısı: {{DegiskenAdi}} olur.
                text = text.Replace($"{{{{{item.Key}}}}}", item.Value ?? string.Empty);
            }
            return text;
        }

        // MailKit ile Gönderim ve Veritabanına Loglama Metodu
        private async Task<bool> SendAndLogAsync(List<string> toEmails, List<string> bccEmails, string subject, string htmlBody, string templateCode)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Yatırım Koç", _mailSettings.FromEmail));

            // To (Kime) Ekle
            if (toEmails != null)
            {
                foreach (var to in toEmails)
                    email.To.Add(MailboxAddress.Parse(to));
            }

            // BCC (Gizli Kime) Ekle
            if (bccEmails != null)
            {
                foreach (var bcc in bccEmails)
                    email.Bcc.Add(MailboxAddress.Parse(bcc));
            }

            email.Subject = subject;

            // HTML Formatında Gövde
            var builder = new BodyBuilder { HtmlBody = htmlBody };
            email.Body = builder.ToMessageBody();

            bool isSuccess = false;
            string errorMessage = null;

            // SMTP İşlemleri
            try
            {
                using var smtp = new SmtpClient();

                // Güvenlik ayarı (SSL/TLS)
                var secureSocketOptions = _mailSettings.UseSSL ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;

                await smtp.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort, secureSocketOptions);
                await smtp.AuthenticateAsync(_mailSettings.SmtpUser, _mailSettings.SmtpPass);

                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                errorMessage = ex.Message; // Hata alırsak DB'ye yazmak için yakalıyoruz
            }

            // ---- LOGLAMA İŞLEMİ (MailLog Tablosuna Yazma) ----
            var primaryReceiver = toEmails?.Count > 0 ? string.Join(",", toEmails) : (bccEmails?.Count > 0 ? $"Toplu Gönderim (BCC: {bccEmails.Count} kişi)" : "Bilinmeyen");

            var mailLog = new MailLog
            {
                ToEmail = primaryReceiver,
                Subject = subject,
                TemplateCode = templateCode,
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage,
                CreatedAt = DateTime.UtcNow
            };

            // Not: IApplicationDbContext içinde MailLogs dbset ismin farklıysa (örn: Logs) aşağıyı ona göre düzenleyebilirsin
            _context.MailLogs.Add(mailLog);
            await _context.SaveChangesAsync(default);

            return isSuccess;
        }
    }
}
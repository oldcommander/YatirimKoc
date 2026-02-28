using System.Collections.Generic;
using System.Threading.Tasks;

namespace YatirimKoc.Application.Interfaces
{
    public interface IMailService
    {
        // Tek bir kişiye mail gönderir (Örn: OTP, Şifre Sıfırlama)
        Task<bool> SendEmailAsync(string toEmail, string templateCode, Dictionary<string, string> placeholders);

        // Toplu mail gönderir (Örn: Tüm aktif abonelere fırsat bülteni)
        Task<bool> SendBulkEmailAsync(List<string> bccEmails, string templateCode, Dictionary<string, string> placeholders);
    }
}
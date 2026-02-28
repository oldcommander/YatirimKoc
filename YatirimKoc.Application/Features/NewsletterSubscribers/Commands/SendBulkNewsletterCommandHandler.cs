using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Interfaces;

namespace YatirimKoc.Application.Features.NewsletterSubscribers.Commands
{
    public class SendBulkNewsletterCommandHandler : IRequestHandler<SendBulkNewsletterCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMailService _mailService;

        public SendBulkNewsletterCommandHandler(IApplicationDbContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        public async Task<bool> Handle(SendBulkNewsletterCommand request, CancellationToken cancellationToken)
        {
            // 1. Sadece "Aktif" abonelerin mail adreslerini veritabanından liste olarak çek
            var activeEmails = await _context.NewsletterSubscribers
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Select(x => x.Email)
                .ToListAsync(cancellationToken);

            // Eğer hiç aktif abone yoksa işlemi boşuna yorma
            if (!activeEmails.Any())
                return false;

            // 2. Mail servisini tetikle (Toplu gönderim - BCC)
            var isSuccess = await _mailService.SendBulkEmailAsync(activeEmails, request.TemplateCode, request.Placeholders);

            return isSuccess;
        }
    }
}
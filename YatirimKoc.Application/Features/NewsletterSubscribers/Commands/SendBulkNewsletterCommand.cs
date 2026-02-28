using MediatR;
using System.Collections.Generic;

namespace YatirimKoc.Application.Features.NewsletterSubscribers.Commands
{
    public class SendBulkNewsletterCommand : IRequest<bool>
    {
        public string TemplateCode { get; set; }

        // Şablondaki {{DegiskenAdi}} kısımlarını dolduracak veriler
        public Dictionary<string, string> Placeholders { get; set; } = new();
    }
}
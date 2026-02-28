using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Features.NewsletterSubscribers.Dtos;

namespace YatirimKoc.Application.Features.NewsletterSubscribers.Queries
{
    public class GetMailLogDetailQuery : IRequest<MailLogDto>
    {
        public Guid Id { get; set; }
    }

    public class GetMailLogDetailQueryHandler : IRequestHandler<GetMailLogDetailQuery, MailLogDto>
    {
        private readonly IApplicationDbContext _context;

        public GetMailLogDetailQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MailLogDto> Handle(GetMailLogDetailQuery request, CancellationToken cancellationToken)
        {
            var log = await _context.MailLogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (log == null) return null;

            return new MailLogDto
            {
                Id = log.Id,
                IsSuccess = log.IsSuccess,
                ErrorMessage = log.ErrorMessage,
                CreatedAt = log.CreatedAt,
                Subject = log.Subject,
                ToEmail = log.ToEmail,
                TemplateCode = log.TemplateCode
            };
        }
    }
}
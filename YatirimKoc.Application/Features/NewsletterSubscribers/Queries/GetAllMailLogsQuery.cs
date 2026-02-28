using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Common.Models;
using YatirimKoc.Application.Features.NewsletterSubscribers.Dtos;

namespace YatirimKoc.Application.Features.NewsletterSubscribers.Queries
{
    public class GetAllMailLogsQuery : IRequest<PaginatedList<MailLogDto>>
    {
        public string SearchTerm { get; set; }
        public bool? IsSuccess { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;
    }

    public class GetAllMailLogsQueryHandler : IRequestHandler<GetAllMailLogsQuery, PaginatedList<MailLogDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllMailLogsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<MailLogDto>> Handle(GetAllMailLogsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.MailLogs.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(x => x.ToEmail.Contains(request.SearchTerm) || x.Subject.Contains(request.SearchTerm));
            }

            if (request.IsSuccess.HasValue)
            {
                query = query.Where(x => x.IsSuccess == request.IsSuccess.Value);
            }

            var count = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new MailLogDto
                {
                    Id = x.Id,
                    ToEmail = x.ToEmail,
                    Subject = x.Subject,
                    TemplateCode = x.TemplateCode,
                    IsSuccess = x.IsSuccess,
                    ErrorMessage = x.ErrorMessage,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new PaginatedList<MailLogDto>(items, count, request.PageNumber, request.PageSize);
        }
    }
}
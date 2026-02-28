using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Features.NewsletterSubscribers.Dtos;

namespace YatirimKoc.Application.Features.NewsletterSubscribers.Queries
{
    // Sayfalama bilgilerini tutacak özel dönüş sınıfımız
    public class PaginatedSubscriberResult
    {
        public List<SubscriberDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }

    public class GetAllSubscribersQuery : IRequest<PaginatedSubscriberResult>
    {
        public string SearchTerm { get; set; }
        public bool? IsActive { get; set; }
        public string SortOrder { get; set; } // "date_desc", "date_asc"
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetAllSubscribersQueryHandler : IRequestHandler<GetAllSubscribersQuery, PaginatedSubscriberResult>
    {
        private readonly IApplicationDbContext _context;

        public GetAllSubscribersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedSubscriberResult> Handle(GetAllSubscribersQuery request, CancellationToken cancellationToken)
        {
            var query = _context.NewsletterSubscribers.AsNoTracking();

            // 1. Arama (Search)
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(x => x.Email.Contains(request.SearchTerm));
            }

            // 2. Filtreleme (Aktif/Pasif)
            if (request.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == request.IsActive.Value);
            }

            // 3. Sıralama (Sort)
            query = request.SortOrder switch
            {
                "date_asc" => query.OrderBy(x => x.CreatedDate),
                _ => query.OrderByDescending(x => x.CreatedDate), // Varsayılan: En yeniler önce
            };

            // 4. Sayfalama (Pagination)
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.Skip((request.PageNumber - 1) * request.PageSize)
                                   .Take(request.PageSize)
                                   .Select(x => new SubscriberDto
                                   {
                                       Id = x.Id,
                                       Email = x.Email,
                                       IsActive = x.IsActive,
                                       CreatedDate = x.CreatedDate
                                   })
                                   .ToListAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new PaginatedSubscriberResult
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = request.PageNumber,
                TotalPages = totalPages
            };
        }
    }
}
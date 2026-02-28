using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Common.Models;
using YatirimKoc.Application.Features.Reservations.Dtos;
using YatirimKoc.Domain.Enums;

namespace YatirimKoc.Application.Features.Reservations.Queries;

public class GetAllReservationsQuery : IRequest<PaginatedList<ReservationDto>>
{
    public string? SearchTerm { get; set; }
    public ReservationStatus? Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetAllReservationsQueryHandler : IRequestHandler<GetAllReservationsQuery, PaginatedList<ReservationDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllReservationsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ReservationDto>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Reservations.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.ToLower();
            query = query.Where(x => x.FullName.ToLower().Contains(search) ||
                                     x.Email.ToLower().Contains(search));
        }

        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status.Value);
        }

        var count = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.ReservationDate)
            .ThenBy(x => x.TimeSlot)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new ReservationDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Phone = x.Phone,
                ReservationDate = x.ReservationDate,
                TimeSlot = x.TimeSlot,
                Status = x.Status,
                CreatedAt = x.CreatedAt
            }).ToListAsync(cancellationToken);

        return new PaginatedList<ReservationDto>(items, count, request.PageNumber, request.PageSize);
    }
}
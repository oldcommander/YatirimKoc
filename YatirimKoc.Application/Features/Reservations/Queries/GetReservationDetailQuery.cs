using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Features.Reservations.Dtos;

namespace YatirimKoc.Application.Features.Reservations.Queries;

public class GetReservationDetailQuery : IRequest<ReservationDto?>
{
    public Guid Id { get; set; }
    public GetReservationDetailQuery(Guid id) { Id = id; }
}

public class GetReservationDetailQueryHandler : IRequestHandler<GetReservationDetailQuery, ReservationDto?>
{
    private readonly IApplicationDbContext _context;
    public GetReservationDetailQueryHandler(IApplicationDbContext context) { _context = context; }

    public async Task<ReservationDto?> Handle(GetReservationDetailQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.Reservations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (res == null) return null;

        return new ReservationDto
        {
            Id = res.Id,
            FullName = res.FullName,
            Email = res.Email,
            Phone = res.Phone,
            ReservationDate = res.ReservationDate,
            TimeSlot = res.TimeSlot,
            Status = res.Status,
            Note = res.Note,
            AdminProfileId = res.AdminProfileId,
            CreatedAt = res.CreatedAt
        };
    }
}
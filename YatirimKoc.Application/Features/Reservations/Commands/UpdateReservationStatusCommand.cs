using MediatR;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Domain.Enums;

namespace YatirimKoc.Application.Features.Reservations.Commands;

public class UpdateReservationStatusCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public ReservationStatus NewStatus { get; set; }
}

public class UpdateReservationStatusCommandHandler : IRequestHandler<UpdateReservationStatusCommand, bool>
{
    private readonly IApplicationDbContext _context;
    public UpdateReservationStatusCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<bool> Handle(UpdateReservationStatusCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _context.Reservations.FindAsync(new object[] { request.Id }, cancellationToken);
        if (reservation == null) return false;

        reservation.Status = request.NewStatus;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
using MediatR;
using YatirimKoc.Application.Abstractions.Persistence;

namespace YatirimKoc.Application.Features.Reservations.Commands;

public class DeleteReservationCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public DeleteReservationCommand(Guid id) { Id = id; }
}

public class DeleteReservationCommandHandler : IRequestHandler<DeleteReservationCommand, bool>
{
    private readonly IApplicationDbContext _context;
    public DeleteReservationCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<bool> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _context.Reservations.FindAsync(new object[] { request.Id }, cancellationToken);
        if (reservation == null) return false;

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
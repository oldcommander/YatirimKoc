using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Domain.Enums;

namespace YatirimKoc.Application.Features.ContactMessages.Commands;

public class UpdateContactMessageStatusCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public ContactMessageStatus NewStatus { get; set; }
}

public class UpdateContactMessageStatusCommandHandler : IRequestHandler<UpdateContactMessageStatusCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateContactMessageStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateContactMessageStatusCommand request, CancellationToken cancellationToken)
    {
        var message = await _context.ContactMessages.FindAsync(new object[] { request.Id }, cancellationToken);

        if (message == null) return false;

        message.Status = request.NewStatus;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
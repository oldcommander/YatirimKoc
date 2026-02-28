using MediatR;
using YatirimKoc.Application.Abstractions.Persistence;

namespace YatirimKoc.Application.Features.ContactMessages.Commands;

public class DeleteContactMessageCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public DeleteContactMessageCommand(Guid id) { Id = id; }
}

public class DeleteContactMessageCommandHandler : IRequestHandler<DeleteContactMessageCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteContactMessageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteContactMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _context.ContactMessages.FindAsync(new object[] { request.Id }, cancellationToken);

        if (message == null) return false;

        _context.ContactMessages.Remove(message);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
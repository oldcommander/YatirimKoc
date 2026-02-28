using MediatR;
using YatirimKoc.Application.Abstractions.Persistence;

namespace YatirimKoc.Application.Features.Mails.Commands;

public class DeleteMailCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public DeleteMailCommand(Guid id) { Id = id; }
}

public class DeleteMailCommandHandler : IRequestHandler<DeleteMailCommand, bool>
{
    private readonly IApplicationDbContext _context;
    public DeleteMailCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<bool> Handle(DeleteMailCommand request, CancellationToken cancellationToken)
    {
        var mail = await _context.Mails.FindAsync(new object[] { request.Id }, cancellationToken);
        if (mail == null) return false;

        _context.Mails.Remove(mail);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
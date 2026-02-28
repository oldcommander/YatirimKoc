using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Domain.Entities.Mails;

namespace YatirimKoc.Application.Features.Mails.Commands;

public class CreateMailCommand : IRequest<bool>
{
    public string Email { get; set; } = null!;
}

public class CreateMailCommandHandler : IRequestHandler<CreateMailCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public CreateMailCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CreateMailCommand request, CancellationToken cancellationToken)
    {
        // Aynı mail adresi zaten var mı kontrolü
        bool exists = await _context.Mails.AnyAsync(x => x.Email.ToLower() == request.Email.ToLower(), cancellationToken);
        if (exists) return false;

        var mail = new YatirimKoc.Domain.Entities.Mails.Mails
        {
            Email = request.Email,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Mails.Add(mail);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
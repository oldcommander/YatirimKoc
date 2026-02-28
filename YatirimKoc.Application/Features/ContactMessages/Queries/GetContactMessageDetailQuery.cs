using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Features.ContactMessages.Dtos;

namespace YatirimKoc.Application.Features.ContactMessages.Queries;

public class GetContactMessageDetailQuery : IRequest<ContactMessageDto?>
{
    public Guid Id { get; set; }

    public GetContactMessageDetailQuery(Guid id)
    {
        Id = id;
    }
}

public class GetContactMessageDetailQueryHandler : IRequestHandler<GetContactMessageDetailQuery, ContactMessageDto?>
{
    private readonly IApplicationDbContext _context;

    public GetContactMessageDetailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ContactMessageDto?> Handle(GetContactMessageDetailQuery request, CancellationToken cancellationToken)
    {
        var message = await _context.ContactMessages
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (message == null) return null;

        return new ContactMessageDto
        {
            Id = message.Id,
            FullName = message.FullName,
            Email = message.Email,
            Phone = message.Phone,
            Subject = message.Subject,
            Message = message.Message,
            Status = message.Status,
            CreatedAt = message.CreatedAt
        };
    }
}
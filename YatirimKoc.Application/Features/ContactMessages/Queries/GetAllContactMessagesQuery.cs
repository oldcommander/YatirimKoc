using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Features.ContactMessages.Dtos;

namespace YatirimKoc.Application.Features.ContactMessages.Queries;

public class GetAllContactMessagesQuery : IRequest<List<ContactMessageDto>>
{
}

public class GetAllContactMessagesQueryHandler : IRequestHandler<GetAllContactMessagesQuery, List<ContactMessageDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllContactMessagesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ContactMessageDto>> Handle(GetAllContactMessagesQuery request, CancellationToken cancellationToken)
    {
        return await _context.ContactMessages
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ContactMessageDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Phone = x.Phone,
                Subject = x.Subject,
                Status = x.Status,
                CreatedAt = x.CreatedAt
            }).ToListAsync(cancellationToken);
    }
}
using MediatR;

namespace YatirimKoc.Application.Features.Listings.Commands;

public class DeleteListingCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
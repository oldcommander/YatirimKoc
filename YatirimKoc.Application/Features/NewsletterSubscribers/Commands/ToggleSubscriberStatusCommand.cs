using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using YatirimKoc.Application.Abstractions.Persistence;

namespace YatirimKoc.Application.Features.NewsletterSubscribers.Commands
{
    public class ToggleSubscriberStatusCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class ToggleSubscriberStatusCommandHandler : IRequestHandler<ToggleSubscriberStatusCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public ToggleSubscriberStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ToggleSubscriberStatusCommand request, CancellationToken cancellationToken)
        {
            var subscriber = await _context.NewsletterSubscribers.FindAsync(new object[] { request.Id }, cancellationToken);
            if (subscriber == null) return false;

            subscriber.IsActive = !subscriber.IsActive;
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
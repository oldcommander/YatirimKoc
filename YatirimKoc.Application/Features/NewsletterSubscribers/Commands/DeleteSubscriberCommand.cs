using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using YatirimKoc.Application.Abstractions.Persistence;

namespace YatirimKoc.Application.Features.NewsletterSubscribers.Commands
{
    public class DeleteSubscriberCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteSubscriberCommandHandler : IRequestHandler<DeleteSubscriberCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public DeleteSubscriberCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteSubscriberCommand request, CancellationToken cancellationToken)
        {
            var subscriber = await _context.NewsletterSubscribers.FindAsync(new object[] { request.Id }, cancellationToken);
            if (subscriber == null) return false;

            _context.NewsletterSubscribers.Remove(subscriber);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Domain.Entities.Mails;

namespace YatirimKoc.Application.Features.NewsletterSubscribers.Commands
{
    public class CreateSubscriberCommand : IRequest<Guid>
    {
        public string Email { get; set; }
    }

    public class CreateSubscriberCommandHandler : IRequestHandler<CreateSubscriberCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateSubscriberCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateSubscriberCommand request, CancellationToken cancellationToken)
        {
            var subscriber = new NewsletterSubscriber
            {
                Email = request.Email,
                IsActive = true
            };

            _context.NewsletterSubscribers.Add(subscriber);
            await _context.SaveChangesAsync(cancellationToken);

            return subscriber.Id;
        }
    }
}
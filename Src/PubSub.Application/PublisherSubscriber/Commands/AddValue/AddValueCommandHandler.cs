using MediatR;
using PubSub.Domain.Interfaces;

namespace PubSub.Application.PublisherSubscriber.Commands.AddValue
{
    internal class AddValueCommandHandler : IRequestHandler<AddValueCommand>
    {
        private readonly IPublisherSubscriberManager _publisherSubscriberManager;

        public AddValueCommandHandler(IPublisherSubscriberManager publisherSubscriberManager) 
        { 
            _publisherSubscriberManager = publisherSubscriberManager ?? throw new ArgumentNullException(nameof(publisherSubscriberManager));
        }

        Task IRequestHandler<AddValueCommand>.Handle(AddValueCommand request, CancellationToken cancellationToken)
        {
            _publisherSubscriberManager.AddValue(request.Key, request.Value);

            return Task.CompletedTask;
        }
    }
}
using MediatR;
using PubSub.Domain.Interfaces;

namespace PubSub.Application.PublisherSubscriber.Queries.WaitForValueAsync
{
    internal class WaitForValueAsyncQueryHandler : IRequestHandler<WaitForValueAsyncQuery, WaitForValueAsyncQueryResponse>
    {
        private readonly IPublisherSubscriberManager _publisherSubscriberManager;

        public WaitForValueAsyncQueryHandler(IPublisherSubscriberManager publisherSubscriberManager)
        {
            _publisherSubscriberManager = publisherSubscriberManager ?? throw new ArgumentNullException(nameof(publisherSubscriberManager));
        }

        async Task<WaitForValueAsyncQueryResponse> IRequestHandler<WaitForValueAsyncQuery, WaitForValueAsyncQueryResponse>.Handle(WaitForValueAsyncQuery request, CancellationToken cancellationToken)
        {
            var result = await _publisherSubscriberManager.WaitForValueAsync(request.Key, request.MillisecondsWait, cancellationToken);

            return new WaitForValueAsyncQueryResponse()
            {
                Value = result.Value,
                Success = result.Success,
                ErrorMessage = result.ErrorMessage
            };
        }
    }
}
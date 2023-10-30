using MediatR;

namespace PubSub.Application.PublisherSubscriber.Queries.WaitForValueAsync
{
    public record class WaitForValueAsyncQuery(string Key, int MillisecondsWait) : IRequest<WaitForValueAsyncQueryResponse>;    
}

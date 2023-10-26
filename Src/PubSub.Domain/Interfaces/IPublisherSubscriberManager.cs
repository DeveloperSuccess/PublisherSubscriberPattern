using PubSub.Domain.ValueObjects;

namespace PubSub.Domain.Interfaces
{
    public interface IPublisherSubscriberManager
    {
        void AddValue(string key, string value);
        Task<WaitForValueResponse> WaitForValueAsync(string key, int millisecondsWait, CancellationToken cancellationToken);
    }
}

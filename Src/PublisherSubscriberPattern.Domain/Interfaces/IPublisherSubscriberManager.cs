using PublisherSubscriberPattern.Domain.ValueObjects;

namespace PublisherSubscriberPattern.Domain.Interfaces
{
    public interface IPublisherSubscriberManager
    {
        void AddValue(string key, string value);
        Task<WaitForValueResponse> WaitForValueAsync(string key, int millisecondsWait, CancellationToken cancellationToken);
    }
}

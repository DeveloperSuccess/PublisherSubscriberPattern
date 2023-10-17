namespace PublisherSubscriberPattern
{
    public interface IPublisherSubscriberManager
    {
        void AddValue(string key, string value);
        Task<WaitForValueResponse> WaitForValueAsync(string key, int millisecondsWait);
    }
}

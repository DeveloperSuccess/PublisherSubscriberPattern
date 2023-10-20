namespace PublisherSubscriberPattern.Models
{
    public class SubscriberModel
    {
        public required string Key { get; set; }
        public required TaskCompletionSource<string> Value { get; set; }
    }
}

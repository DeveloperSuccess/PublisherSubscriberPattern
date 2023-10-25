namespace PublisherSubscriberPattern.Domain.ValueObjects

{
    internal class Subscriber
    {
        public required string Key { get; set; }
        public required TaskCompletionSource<string> Value { get; set; }
    }
}
